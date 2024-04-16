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

    public Draw()
    {
        SetupUI();
    }

    private void SetupUI()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.Font = new Font("Arial", 12, FontStyle.Bold);
        this.ForeColor = Color.White;
        this.BackColor = Color.Black;

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
                Pen pen = new Pen(color);
                g.DrawLine(pen, previousPoint, e.Location);
                previousPoint = e.Location;
            }
            pb.Invalidate();
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

    private void OpenRegisterForm()
    {
        Register registerForm = new Register();
        registerForm.Show();
        this.Hide();
    }
}

