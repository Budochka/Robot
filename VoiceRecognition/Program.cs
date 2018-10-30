using System;
using System.Globalization;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;

namespace VoiceRecognition
{
    class Program
    {
        // Handle the SpeechRecognized event.windows 
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Grammar({0}): {1}", e.Result.Grammar.Name, e.Result.Text);
        }

        static void Main(string[] args)
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.SetOutputToDefaultAudioDevice();

            var ci = new CultureInfo("ru-RU");
            using (var sre = new SpeechRecognitionEngine(ci))
            {
                sre.SetInputToDefaultAudioDevice();

                GrammarBuilder gb = new GrammarBuilder(new Choices(new string[] { "налево", "направо", "вперед", "назад" }));
                gb.Culture = ci;
                Grammar g = new Grammar(gb);

                sre.LoadGrammarAsync(g);
                sre.RecognizeAsync(RecognizeMode.Multiple);
                sre.SpeechRecognized += recognizer_SpeechRecognized;
            }

            Console.ReadLine();
        }
    }
}
