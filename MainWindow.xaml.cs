using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace WpfApp31
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TreeViewItem rootNodePdf = new TreeViewItem();
            rootNodePdf.Header = Pdf_File_Path;
            treeView_pdf.Items.Add(rootNodePdf);

            TreeViewItem rootNodeImage = new TreeViewItem();
            rootNodeImage.Header = Image_File_Path;
            treeView_image.Items.Add(rootNodeImage);
        }
        public string Pdf_File_Path = @"C:\\Users\\Пользователь\\source\\repos\\WpfApp31\\WpfApp31\\PDF_Files";
        public string Image_File_Path = @"C:\\Users\\Пользователь\\source\\repos\\WpfApp31\\WpfApp31\\Image_Files";
        
        private void new_file()
        {
            try
            {
                // Проверяем существование папки
                if (!Directory.Exists(Pdf_File_Path))
                {
                    // Создаем каталог для папки
                    Directory.CreateDirectory(Pdf_File_Path);
                }
                if (!Directory.Exists(Image_File_Path))
                {
                    // Создаем каталог для папки
                    Directory.CreateDirectory(Image_File_Path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании каталога: " + ex.Message);
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            TreeViewItem rootNode = new TreeViewItem();
            if (files.Length > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(files[0]).ToLower();
                if (fileExtension == ".jpg")
                {
                    foreach (string file in files)
                    {
                        string fileName = System.IO.Path.GetFileName(file);
                        string destinationPath = System.IO.Path.Combine(Image_File_Path, fileName);
                        System.IO.File.Copy(file, destinationPath, true);
                    }
                    
                    rootNode.Header = Image_File_Path; //// Добавление корневого элемента в TreeView
                    treeView_image.Items.Add(rootNode); // Заполнение TreeView
                }
                else if (fileExtension == ".pdf")
                {
                    PdfView.Load(files[0]);
                    image.Visibility = Visibility.Hidden;
                    PdfView.Visibility = Visibility.Visible;

                    string new_Pdf_FilePath = System.IO.Path.Combine(Pdf_File_Path, System.IO.Path.GetFileName(files[0]));
                    System.IO.File.Move(files[0], new_Pdf_FilePath);

                    rootNode.Header = Pdf_File_Path;//// Добавление корневого элемента в TreeView
                    treeView_pdf.Items.Add(rootNode); // Заполнение TreeView
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            string[] pdf_files = Directory.GetFiles(Pdf_File_Path, "*.pdf");
            pdf_List.ItemsSource = pdf_files; // загрузка списка .pdf файлов

            string[] image_files = Directory.GetFiles(Image_File_Path, "*.jpg");
            image_List.ItemsSource = image_files; // загрузка списка .jpg файлов
        }
        private void pdf_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFile_pdf = pdf_List.SelectedItem as string; // отслеживание пути pdf файла
            if (selectedFile_pdf != null)
            {
                PdfView.Load(selectedFile_pdf); // вызов pdf файла
            }
        }

        private void image_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFile_image = image_List.SelectedItem as string; // отслеживание пути pdf файла
            if (selectedFile_image != null)
            {
                BitmapImage bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFile_image);
                bitmap.EndInit();

                image.Source = bitmap;

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
