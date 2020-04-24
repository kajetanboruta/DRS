using AdditionalProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AdditionalProject.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ManageListView.xaml
    /// </summary>
    public partial class ManageListView : Page
    {

        public MainView MainView;
        public ManageListView()
        {
            InitializeComponent();
            LoadFunction();
        }

        public ManageListView(MainView _mainView)
        {
            MainView = _mainView;
            InitializeComponent();
            LoadFunction();
        }
        ObservableCollection<Student> _students = new ObservableCollection<Student>();
        ObservableCollection<ClassGroup> _classGroupsCollection = new ObservableCollection<ClassGroup>();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Context ctx = new Context();
            await ctx.Database.EnsureCreatedAsync();

            Student student = new Student
            {
                FirstName = firstname_Field.Text,
                LastName = lastname_Field.Text,
                AlbumNumber = albumNo_Field.Text,
                GroupID = Convert.ToInt32(studentGroup_Field.Text)
            };
            try
            {
                await ctx.Students.AddAsync(student);
                await ctx.SaveChangesAsync();

                MessageBox.Show($"Do bazy danych dodano studenta o nr albumu: {student.AlbumNumber}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void LoadFunction()
        {
            Context ctx = new Context();
            await ctx.Database.EnsureCreatedAsync();


            foreach (var item in ctx.ClassGroups)
            {
                _classGroupsCollection.Add(item);
            }
            groups_Datagrid.ItemsSource = _classGroupsCollection;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClassGroup classGroup = new ClassGroup();
            if (groups_Datagrid.SelectedItems != null)
            {
                studentGroup_Field.Text = (groups_Datagrid.SelectedItem as ClassGroup).ID.ToString();
                groups_Button.IsEnabled = true;
            }
            else
                groups_Button.IsEnabled = false;
                
        }

        private async void groups_Button_Click(object sender, RoutedEventArgs e)
        {
            Context ctx = new Context();
            await ctx.Database.EnsureCreatedAsync();
            ClassGroup _classGroup = new ClassGroup();
            _classGroup.Name = groupName_Field.Text;
            try
            {
                await ctx.ClassGroups.AddAsync(_classGroup);
                await ctx.SaveChangesAsync();
                MessageBox.Show($"Dodano grupę: { _classGroup.Name } do bazy danych.");
                _classGroupsCollection.Add(_classGroup);
                groups_Datagrid.ItemsSource = _classGroupsCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void groups_Button_Click_1(object sender, RoutedEventArgs e)
        {
            students_Datagrid.Items.Clear();
            Context ctx = new Context();
            await ctx.Database.EnsureCreatedAsync();
            try
            {
                var selectedGroup = (groups_Datagrid.SelectedItem as ClassGroup);

                foreach (var item in ctx.Students)
                {
                    if (item.GroupID == selectedGroup.ID)
                        _students.Add(item);
                }
                students_Datagrid.ItemsSource = _students;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RandomizeView randomizeView = new RandomizeView(this);
            MainView.MainFrame.Navigate(randomizeView);

            randomizeView.blockOfStudents.Children.Clear();
            List<Student> _students = new List<Student>();

            foreach (Student item in students_Datagrid.Items)
            {
                if (item.IsAbsent)
                    continue;

                _students.Add(item);
            }

            var maxWidth = MainView.ActualWidth;
            foreach (var item in _students)
            {
                Grid studentBlock = new Grid()
                {
                    Name = $"_{item.AlbumNumber}",
                    Height = 100,
                    Width = (double)maxWidth / (double)_students.Count,
                    ToolTip = $"{item.FirstName} {item.LastName}",
                    Background = randomizeView.PickBrush()
                };
                randomizeView.blockOfStudents.Children.Add(studentBlock);
            }

            MainView.Width += 20;
        }
    }
}
