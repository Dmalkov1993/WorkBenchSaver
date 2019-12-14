using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

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
                Console.WriteLine("Укажите название файла, который будем копировать");

                var EwbFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.ewb");

                int indexInEwbFiles = 0;
                foreach (string EwbFile in EwbFiles)
                {
                    Console.WriteLine("[" + indexInEwbFiles + "] " + Path.GetFileName(EwbFile));
                    indexInEwbFiles++;
                }

                int SelectedEwbFile = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Выбран для бекапа файл: " + Path.GetFileName(EwbFiles[SelectedEwbFile]));
                Console.WriteLine("Для остановки данной процедуры нажмите Ctrl + C, или красный крестик.");

                string SelectedEwbFilePath = EwbFiles[SelectedEwbFile];

                FileInfo fileInfo = new FileInfo(SelectedEwbFilePath);

                //Запоминаем начальный размер файла
                long LastSize = fileInfo.Length;
                long currentSize = 0;

                //Начинаем бекап
                //while (Console.ReadKey().Key != ConsoleKey.Escape)
                while (true)
                {
                    fileInfo.Refresh();
                    currentSize = fileInfo.Length;

                    if (currentSize != LastSize && currentSize > 5000)
                    //if (true)
                    {
                        string ooo = GetDestinationFilePath(SelectedEwbFilePath);

                        File.Copy(SelectedEwbFilePath, ooo);

                        Console.WriteLine("Файл был изменён. LastSize: " + LastSize / 1024 + " Кб,  currentSize: " + currentSize / 1024 + " Кб.");
                        LastSize = currentSize;
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
            }
        }

        static string GetDestinationFilePath(string FullPathFile)
        {
            return Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(FullPathFile) + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + Path.GetExtension(FullPathFile);
        }
    }
}
