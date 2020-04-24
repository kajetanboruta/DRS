using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AdditionalProject.Models;

namespace AdditionalProject.Views
{
    /// <summary>
    /// Logika interakcji dla klasy RandomizeView.xaml
    /// </summary>
    public partial class RandomizeView : Page
    {
        public RandomizeView()
        {
            InitializeComponent();
        }
        public ManageListView ManageListView;

        public RandomizeView(ManageListView _manageListView)
        {
            ManageListView = _manageListView;
            InitializeComponent();
        }

        //global variables

        //rnd is our global random variable.
        readonly private Random rnd = new Random();
        //grid to make life easier.
        private Grid winner;

        /// <summary>
        /// Generates random SolidColorBrush
        /// </summary>
        /// <returns></returns>
        public Brush PickBrush()
        {
            _ = Brushes.Transparent;
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();

            var random = rnd.Next(properties.Length);

            Brush result = (Brush)properties[random].GetValue(null, null);
            return result;
        }

        /// <summary>
        /// Loads students from chosen list to equivalent grids
        /// </summary>
        /// <param name="maxWidth"></param>
        public void LoadStudentsToList(double maxWidth, List<Student> _students)
        {
            blockOfStudents.Children.Clear();
            _students = new List<Student>();

            foreach (var item in _students)
            {
                Grid studentBlock = new Grid()
                {
                    Name = $"_{item.AlbumNumber}",
                    Height = 100,
                    Width = (double)maxWidth / (double)_students.Count,
                    ToolTip = $"{item.FirstName} {item.LastName}",
                    Background = PickBrush(),
                };
                blockOfStudents.Children.Add(studentBlock);
            }

            //Student student = new Student("055289", "kajetan", "Boruta");
            //Student student2 = new Student("055288", "Adam", "Grzesiek");
            //Student student3 = new Student("055287", "Sebastian", "Domagała");
            //Student student4 = new Student("055387", "Sadsad", "Domagała");
            //Student student5 = new Student("052342", "asd", "Dweqewq");
            //Student student6 = new Student("053257", "asddn", "egwgew");
            //Student student7 = new Student("065647", "asdad", "sadad");
            //Student student8 = new Student("0552859", "kajetan", "Boruta");
            //Student student9 = new Student("0552838", "Adam", "Grzesiek");
            //Student student10 = new Student("0552287", "Sebastian", "Domagała");
            //Student student11 = new Student("0553187", "Sadsad", "Domagała");
            //Student student12 = new Student("0523642", "asd", "Dweqewq");
            //Student student13 = new Student("0532757", "asddn", "egwgew");
            //Student student14 = new Student("0685647", "asdad", "sadad");

            //List<Student> students = new List<Student>
            //{
            //    student,
            //    student2,
            //    student3,
            //    student4,
            //    student5,
            //    student6,
            //    student7,
            //    student8,
            //    student9,
            //    student10,
            //    student11,
            //    student12,
            //    student13,
            //    student14,
            //};


        }

        /// <summary>
        /// Button event to proc function of loading list
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">click</param>
        //private void LoadList(object sender, RoutedEventArgs e)
        //{
        //    LoadStudentsToList(this.ActualWidth);
        //}

        /// <summary>
        /// Generates random value from range of window width ( don't even ask )
        /// </summary>
        /// <returns></returns>
        private int GenerateIndicatorMovement()
        {
            return rnd.Next(0, (int)ActualWidth);
        }

        /// <summary>
        /// Animates movement of draw mechanic
        /// </summary>
        /// <returns></returns>
        private int MoveIndicator()
        {
            winnerDrawn.Text = "";
            drawButton.IsEnabled = false;  
            int leftMargin = GenerateIndicatorMovement();

            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = randomIndicator.Margin,
                To = new Thickness((double)leftMargin, 0, 0, 0),
            };

            animation.Completed += (s, e) =>
            {
                drawButton.IsEnabled = true;
                winnerDrawn.Visibility = Visibility.Visible;
            };
            
            //Animation of indicator
            randomIndicator.BeginAnimation(MarginProperty, animation);

            return leftMargin;
        }

        /// <summary>
        /// Function takes drawn grid width, and shrinks it ( it's chance of draw in next draws is lessened ) 
        /// Searches for winner, then the value which we shrunk the grid is partialized and then added to every grid ( instead of winner )
        /// to make their chance of draw even.
        /// </summary>
        /// <param name="gridWidth">Drawn grid width</param>
        /// <param name="drawnStudent">Name of grid of winner</param>
        private void FunctionCountWidth(double gridWidth, string drawnStudent)
        {
            double minWidth = ((double)ActualWidth / (double)blockOfStudents.Children.Count) / 8;
            foreach (Grid grid in blockOfStudents.Children)
            {
                if (grid.Name == drawnStudent)
                {
                    if (grid.Width > minWidth)
                    {
                        grid.Width /= 2.0;
                        double diffWidth = (gridWidth - grid.Width) / (blockOfStudents.Children.Count - 1);

                        foreach (Grid gridToEnlarge in blockOfStudents.Children)
                        {
                            if (gridToEnlarge.Name != drawnStudent)
                                gridToEnlarge.Width += diffWidth;
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// This function finds the winner. Really. It finds the indicator position, then checks range of each grid in the panel, 
        /// if it contains the value of left margin of indicator
        /// </summary>
        /// <param name="indicatorPosition">Position of indicator</param>
        /// <returns></returns>
        private Grid DrawWinner(int indicatorPosition)
        {
            int rangeStart = 0;
            foreach (Grid child in blockOfStudents.Children)
            {
                int rangeEnd = (int)child.Width;

                if (Enumerable.Range(rangeStart, rangeEnd).Contains(indicatorPosition))
                    //winnerDrawn.Text = child.ToolTip.ToString();
                    return child;
                else
                    rangeStart += rangeEnd;
            }
            return null;
        }

        /// <summary>
        /// Easy. If there WAS a winner then this function procs.
        /// </summary>
        /// <param name="_winner">Name of winner ( album number )</param>
        /// <returns></returns>
        private Grid DrawWithPreviousWinner(Grid _winner)
        {
            FunctionCountWidth( _winner.Width , _winner.Name);
            return DrawWinner(MoveIndicator());
        }

        /// <summary>
        /// If there was not a winner then DrawWinner is procced. If there wasn't then function DrawWithPreviousWinner will be proced.
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">click</param>
        public void Draw(object sender, RoutedEventArgs e)
        {
            winnerDrawn.Visibility = Visibility.Collapsed;
            //drawButton.IsEnabled = false;
            if (winner == null)
                winner = DrawWinner(MoveIndicator());
            else
                winner = DrawWithPreviousWinner(winner);

            if (winner != null)
                winnerDrawn.Text = "Wygrywa " + winner.ToolTip.ToString() + "!!!";
        }
    }
}
