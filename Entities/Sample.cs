using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Entities
{
    public class Sample
    {
        public string Name { get; set; }
        public string DatabaseID { get; set; }
        public IAudioPlayer Sound { get; set; }
        public Sample(string name, Pitch pitch)
        {
            Name = name;
            LoadSound(pitch);
        }
        public async void PlaySound(int volume, int duration, int pan, int bpm)
        {
            Sound.Balance = pan / 100.0;
            Sound.Volume = volume / 100.0;

            Sound.Play();
            await Task.Delay((int)((15.0 / bpm) * duration * 1000));
            Sound.Stop();
        }
        public async void LoadSound(Pitch pitch)
        {
            if (await FileSystem.AppPackageFileExistsAsync($"{Name}/{pitch}.wav"))
            {
                var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync($"{Name}/{pitch}.wav"));
                Sound = audioPlayer;
            }
        }
    }
}
