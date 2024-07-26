using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
//C:\Users\EnkayPC\AppData\Local\Temp Log Location
namespace VideoMergeApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            axWindowsMediaPlayer1.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(axWindowsMediaPlayer1_PlayStateChange);
            axWindowsMediaPlayer2.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(axWindowsMediaPlayer2_PlayStateChange);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "MP4 Files|*.mp4|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    listBox1.Items.Add(file);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "MP3 Files|*.mp3|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    listBox2.Items.Add(file);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                PlaySelectedVideo();
                if (listBox2.Items.Count >= 1 && listBox1.SelectedIndex == 0)
                {
                    string selectedFile = listBox2.Items[listBox2.Items.Count - 1].ToString();
                    axWindowsMediaPlayer2.URL = selectedFile;
                    axWindowsMediaPlayer2.Ctlcontrols.play();
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                PlaySelectedAudio();
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8) // MediaEnded state
            {
                PlayNextVideo();
            }
        }

        private void axWindowsMediaPlayer2_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8) // MediaEnded state
            {
                PlayNextAudio();
            }
        }

        private void PlaySelectedVideo()
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedFile = listBox1.SelectedItem.ToString();
                axWindowsMediaPlayer1.URL = selectedFile;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private async void PlayNextVideo()
        {
            int currentIndex = listBox1.SelectedIndex;
            if (currentIndex >= 0 && currentIndex < listBox1.Items.Count - 1)
            {
                listBox1.SelectedIndex = currentIndex + 1;
                string nextFile = listBox1.Items[listBox1.SelectedIndex].ToString();
                axWindowsMediaPlayer1.URL = nextFile;
                await Task.Delay(500);  // Wait for half a second
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void PlaySelectedAudio()
        {
            if (listBox2.SelectedItem != null)
            {
                string selectedFile = listBox2.SelectedItem.ToString();
                axWindowsMediaPlayer2.URL = selectedFile;
                axWindowsMediaPlayer2.Ctlcontrols.play();
            }
        }

        private void PlayNextAudio()
        {
            int currentIndex = listBox2.SelectedIndex;
            if (currentIndex >= 0 && currentIndex < listBox2.Items.Count - 1)
            {
                listBox2.SelectedIndex = currentIndex + 1;
                string nextFile = listBox2.Items[listBox2.SelectedIndex].ToString();
                axWindowsMediaPlayer2.URL = nextFile;
                axWindowsMediaPlayer2.Ctlcontrols.play();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Hepsini Silmek İçin Evet, Seçili Satırı Silmek İçin Hayır'a Basınız...",
                    "Silme İşlemi",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    listBox1.Items.Clear();
                }
                else if (result == DialogResult.No)
                {
                    if (listBox1.SelectedIndex != -1)
                    {
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen silmek için bir satır seçin.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Listede silinecek öğe bulunmuyor.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Hepsini Silmek İçin Evet, Seçili Satırı Silmek İçin Hayır'a Basınız...",
                    "Silme İşlemi",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    listBox2.Items.Clear();
                }
                else if (result == DialogResult.No)
                {
                    if (listBox2.SelectedIndex != -1)
                    {
                        listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen silmek için bir satır seçin.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Listede silinecek öğe bulunmuyor.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("ListBox1'de video bulunmuyor.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "MP4 Files|*.mp4",
                Title = "Videoyu Kaydet"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string outputPath = saveFileDialog.FileName;

                string tempDir = Path.Combine(Path.GetTempPath(), "VideoTemp");
                Directory.CreateDirectory(tempDir);

                try
                {
                    string videoListPath = Path.Combine(tempDir, "videoList.txt");
                    using (StreamWriter writer = new StreamWriter(videoListPath))
                    {
                        foreach (string videoFile in listBox1.Items)
                        {
                            writer.WriteLine($"file '{videoFile.Replace("\\", "/")}'");
                        }
                    }

                    string mergedVideoPath = Path.Combine(tempDir, "merged.mp4");
                    string concatCommand = $"-f concat -safe 0 -i \"{videoListPath}\" -c copy \"{mergedVideoPath}\"";
                    ExecuteFFmpegCommand(concatCommand, "Video Birleştirme");

                    if (listBox2.Items.Count > 0)
                    {
                        string audioFile = listBox2.Items[0].ToString().Replace("\\", "/");
                        string addAudioCommand = $"-i \"{mergedVideoPath}\" -i \"{audioFile}\" -c:v copy -c:a aac -map 0:v:0 -map 1:a:0 -strict experimental \"{outputPath}\"";
                        ExecuteFFmpegCommand(addAudioCommand, "Ses Ekleme");
                    }
                    else
                    {
                        File.Move(mergedVideoPath, outputPath);
                        MessageBox.Show("Sadece video başarıyla birleştirildi!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Directory.Delete(tempDir, true);
                    MessageBox.Show("Video Başarıyla Kayıt Edildi! Video Konumu:"+ outputPath.ToString(), "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    richTextBox2.Text = outputPath.ToString();
                }
            }
        }

        private void ExecuteFFmpegCommand(string arguments, string processDescription)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();

                string output = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Error Message Saving
                string logFilePath = Path.Combine(Path.GetTempPath(), $"ffmpeg_log_{processDescription}.txt");
                File.WriteAllText(logFilePath, "Error Output:\n" + output);
                richTextBox1.Text = logFilePath.ToString();
                /*
                if (process.ExitCode != 0)
                {
                    throw new Exception($"FFmpeg komutu başarısız oldu: {output}\nDetaylar için log dosyasını kontrol edin: {logFilePath}");
                }
                else
                {
                    MessageBox.Show($"FFmpeg komut çıktı:\n{output}\nDetaylar için log dosyasını kontrol edin: {logFilePath}", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  
                }
                */
            }
        }



    }
}
