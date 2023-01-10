using System.Windows.Controls;
using System.Windows.Media;

namespace arcoreimg_app.Controls
{
    public partial class EvaluationItemUI : UserControl
    {
        public EvaluationItemUI()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Image
        {
            get { return ImageUri.Text; }
            set { ImageUri.Text = value; }
        }

        public string Title
        {
            get { return ImgTitle.Text; }
            set
            {
                ImgTitle.Text = value;
                ImgTitle.ToolTip = value;
            }
        }

        public int Score
        {
            get { return int.Parse(ImgScore.Text); }
            set
            {
                ImgScore.Text = value + " %";
                LoadingBar.Value = value;
                
                Color foregroundColor = value >= 75 ? Color.FromRgb(0, 122, 0) : Color.FromRgb(122, 0, 0);
                LoadingBar.Foreground = new SolidColorBrush(foregroundColor);
            }
        }
    }
}