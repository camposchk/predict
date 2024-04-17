using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Draw : Form
{
    private PictureBox pb;
    private Graphics g;
    private Point previousPoint;
    private Color color = Color.Black;
    private Button clearButton;
    private Button colorButton;
    private Button registerButton;
    private Label titulo;
    public string Word;
    private string serverUrl = "http://127.0.0.1:5000/cv";

    public Draw()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.Font = new Font("Arial", 12, FontStyle.Bold);
        this.ForeColor = Color.White;
        this.BackColor = Color.Black;

        titulo = new Label
        {
            Text = Word,
            AutoSize = true,
            Font = new Font("Arial", 50, FontStyle.Bold),
            Location = new Point(860, 200),
            Dock = DockStyle.None
        };


        pb = new PictureBox
        {
            Size = new Size(1100, 550),
            Location = new Point(400, 300),
            BackColor = Color.White,
            Dock = DockStyle.None
        };

        Load += (o, e) =>
        {
            Bitmap bitmap = new Bitmap(pb.Width, pb.Height);

            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            this.pb.Image = bitmap;
        };

        pb.Paint += (o, e) =>
        {
            ControlPaint.DrawBorder(e.Graphics, pb.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        };

        pb.MouseDown += (o, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                previousPoint = e.Location;
            }
        };

        pb.MouseMove += (o, e) =>
        {
            if (e.Button == MouseButtons.Left)
            {
                Pen pen = new Pen(color, 5);
                g.DrawLine(pen, previousPoint, e.Location);
                previousPoint = e.Location;
            }
            pb.Invalidate();
        };

        pb.MouseUp += (o, e) =>
        {
            save();
            Connect(serverUrl, "teste", titulo);
        };

        KeyDown += (o, e) =>
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    ClearCanvas();
                    break;
                case Keys.Q:
                    OpenColorDialog();
                    break;
                case Keys.R:
                    OpenRegisterForm();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
        };

        clearButton = new Button
        {
            Text = "Limpar",
            Size = new Size(80, 30),
            Location = new Point(pb.Right + 10, pb.Top)
        };

        clearButton.Click += (o, e) =>
        {
            ClearCanvas();
        };

        colorButton = new Button
        {
            Text = "Cor",
            Size = new Size(80, 30),
            Location = new Point(pb.Right + 10, clearButton.Bottom + 10)
        };

        colorButton.Click += (sender, e) =>
        {
            OpenColorDialog();
        };

        registerButton = new Button
        {
            Text = "Nunca acessou?",
            Size = new Size(200, 30),
            Location = new Point(colorButton.Right + 100, clearButton.Bottom + 700) 
        };

        registerButton.Click += (o, e) =>
        {
            OpenRegisterForm();
        };

        Controls.Add(pb);
        Controls.Add(clearButton);
        Controls.Add(colorButton);
        Controls.Add(registerButton);
        Controls.Add(titulo);
    }

    private void ClearCanvas()
    {
        if (pb.Image != null)
        {
            using (Graphics g = Graphics.FromImage(pb.Image))
            {
                g.Clear(Color.White);
            }
            pb.Invalidate();
        }

        save();
        pb.Invalidate(); 
    }

    private void OpenColorDialog()
    {
        using (ColorDialog colorDialog = new ColorDialog())
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog.Color;
            }
        }
    }

    private void save()
    {
        string filePath = "../test/imagem.png";
        pb.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
    }

    private void OpenRegisterForm()
    {
        Register registerForm = new Register();
        registerForm.Show();
        this.Hide();
    }

    static async Task Connect(string serverUrl, string message, Label label)
    {
        try
        {
            using HttpClient client = new HttpClient();
            var content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain");
            var response = await client.PostAsync(serverUrl, content);

            response.EnsureSuccessStatusCode();

            string word = await response.Content.ReadAsStringAsync();
            label.Invoke((MethodInvoker)delegate {
                label.Text = word;
            });
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"HttpRequestException: {e.Message}");
        }

        Console.WriteLine("\n Press Enter to continue...");
        Console.Read();
    }
}

