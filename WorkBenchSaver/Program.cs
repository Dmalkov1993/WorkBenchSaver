using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

//TODO:
//1) Сделать зеленое мерцание в панели задач при сохранеии
//2) Добавить дату и время в лог на черном экране
//3) Если найдётся только один Ewb файл, то не предлагать выбор файла, так как он один

namespace WorkBenchSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Создадим плеер
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WorkBenchSaver.Resources.OnSaveEvent.wav");
                SoundPlayer player = new SoundPlayer(stream);

                Console.WriteLine("Укажите название файла, который будем копировать" + Environment.NewLine);

                var ewbFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.ewb");

                int indexOfSelectedEwbFile = 0;
                string messageForConsole = "Найден 1 файл, выбираем его по умолч:" + Environment.NewLine;

                if (ewbFiles.Length > 1)
                {
                    for (int indexInEwbFiles = 0; indexInEwbFiles < ewbFiles.Length; indexInEwbFiles++)
                    {
                        Console.WriteLine("[" + indexInEwbFiles + "] " + Path.GetFileName(ewbFiles[indexInEwbFiles]));
                    }

                    indexOfSelectedEwbFile = Convert.ToInt32(Console.ReadLine());
                    messageForConsole = "Выбран для бекапа файл: ";
                }

                Console.WriteLine(messageForConsole + Path.GetFileName(ewbFiles[indexOfSelectedEwbFile]));
                Console.WriteLine(Environment.NewLine + "Для остановки данной процедуры нажмите Ctrl + C, или красный крестик.");

                string selectedEwbFilePath = ewbFiles[indexOfSelectedEwbFile];

                FileInfo fileInfo = new FileInfo(selectedEwbFilePath);

                // Запоминаем начальный размер файла
                long lastSize = fileInfo.Length;
                long currentSize = 0;

                // Начинаем бекап
                while (true)
                {
                    fileInfo.Refresh();
                    currentSize = fileInfo.Length;

                    if (currentSize != lastSize && currentSize > 5000)
                    {
                        File.Copy(selectedEwbFilePath, GetDestinationFilePath(selectedEwbFilePath));

                        player.Play();

                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " Файл был изменён. LastSize: " + lastSize / 1024 + " Кб, currentSize: " + currentSize / 1024 + " Кб.");
                        lastSize = currentSize;
                    }

                    Thread.Sleep(1000);
                    player.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

                Console.ReadKey();
            }
        }

        static string GetDestinationFilePath(string FullPathFile)
        {
            return Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(FullPathFile) + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + Path.GetExtension(FullPathFile);
        }
    }
}
