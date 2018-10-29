using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace VoiceRecognition
{
    class Program
    {

        static void Main(string[] args)
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.SetOutputToDefaultAudioDevice();

            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            sre.SetInputToDefaultAudioDevice();

            Choices choises = new Choices(new string[] {"left", "right", "forward", "backward"});
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choises);
            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }
    }
}
