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

            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new CultureInfo("ru-RU"));
            sre.SetInputToDefaultAudioDevice();

            Choices choises = new Choices(new string[] {"налево", "направо", "вперед", "назад"});
            GrammarBuilder gb = new GrammarBuilder(choises);
            gb.Culture = new CultureInfo("ru-RU");
            Grammar g = new Grammar(gb);
            sre.LoadGrammarAsync(g);
            sre.RecognizeAsync(RecognizeMode.Single);
            sre.SpeechRecognized += recognizer_SpeechRecognized;

            Console.ReadLine();
        }
    }
}
