namespace SITBusiness
{
    using System.Windows.Navigation;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// <see cref="Page"/> class to present information about the current application.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Creates a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();

            spinStoryboard.Begin();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void cmd_MouseEnter(object sender, MouseEventArgs e)
        {
            rotateStoryboard.Stop();
            Storyboard.SetTarget(rotateStoryboard, ((Button)sender).RenderTransform);
            rotateStoryboard.Begin();
        }

        private void cmd_MouseLeave(object sender, MouseEventArgs e)
        {
            unrotateStoryboard.Stop();
            Storyboard.SetTarget(unrotateStoryboard, ((Button)sender).RenderTransform);
            unrotateStoryboard.Begin();
        }

    }
}