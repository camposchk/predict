using System.Drawing;
using System.Windows.Forms;

public partial class Draw : Form
{
    private PictureBox pb;
    private Graphics g;
    private Point previousPoint;
    private Color color = Color.Black; 

    public Draw()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        WindowState = FormWindowState.Maximized;
        BackColor = Color.White;

        pb = new PictureBox
        {
            Dock = DockStyle.Fill
        };
        
        Load += (o, e) =>
        {
            Bitmap bitmap = new(pb.Width, pb.Height);

            g = Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            this.pb.Image = bitmap;
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
            }
        };


        Controls.Add(pb);
    }

    private void ClearCanvas()
    {
        using (Graphics g = Graphics.FromImage(pb.Image))
        {
            g.Clear(Color.White);
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
}

