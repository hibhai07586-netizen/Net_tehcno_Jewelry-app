namespace Jewels_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Mask the password textbox
            textBox2.UseSystemPasswordChar = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            // Validate username and password
            if (ValidateLogin(username, password))
            {
                // Login successful - navigate to dashboard
                home dashboardForm = new home();
                dashboardForm.Show();
                this.Hide();
            }
            else
            {
                // Login failed - show error message
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            // Simple validation - you can replace this with your own logic
            // For example: check against a database, API, or hardcoded credentials
            const string validUsername = "admin";
            const string validPassword = "123";

            return username == validUsername && password == validPassword;
        }
    }
}
