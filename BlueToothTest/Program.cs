﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using static System.Console;

namespace BlueToothTest
{
    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }

    class Program
    {
        private const ulong AdreessLEGO = 95892790903;

        static void ConnectBluetooth()
        {
            var device = AsyncHelpers.RunSync<BluetoothLEDevice>(() => BluetoothLEDevice.FromBluetoothAddressAsync(AdreessLEGO).AsTask());

            if (device != null)
            {
                WriteLine($"BLEWATCHER Found: Name = {device.Name}, Address = {device.BluetoothAddress}, AddressType = {device.BluetoothAddressType}");

                // SERVICES!!

                var services = AsyncHelpers.RunSync<GattDeviceServicesResult>(() => device.GetGattServicesAsync().AsTask());
                //var services = await device.GetGattServicesAsync();
                foreach (var service in services.Services)
                {
                    WriteLine($"Service: {service.Uuid}");
                    //                    var characteristics = await service.GetCharacteristicsAsync();
                    var characteristics = AsyncHelpers.RunSync<GattCharacteristicsResult>(() => service.GetCharacteristicsAsync().AsTask());
                    foreach (var character in characteristics.Characteristics)
                    {
                        WriteLine($"Characteristic: {character.Uuid}");
                    }
                }

                // CHARACTERISTICS!!
//                var characs = await gatt.Services.Single(s => s.Uuid == SAMPLESERVICEUUID).GetCharacteristicsAsync();
//                var charac = characs.Single(c => c.Uuid == SAMPLECHARACUUID);
//                await charac.WriteValueAsync(SOMEDATA);
            }
        }

        static void Main(string[] args)
        {
            ConnectBluetooth();

            // Close on key press
            ReadLine();
        }
    }
}
