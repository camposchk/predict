using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

public partial class Register : Form
{
    private Label nameLabel;
    private TextBox nome;
    private Button submitButton;
    private PictureBox signaturePictureBox;
    private Label assinatura;
    private Graphics g;
    private PictureBox pb;

    private Point previousPoint;
    private Pen pen;
    private Button clearButton;
    private Bitmap canvasImage;

    public Register()
    {
        SetupUI();
        SetupForm();
    }

    private void SetupUI()
    {
        canvasImage = new Bitmap(500, 400);

        nameLabel = new Label
        {
            Text = "Nome:",
            AutoSize = true
        };

        nome = new TextBox
        {
            Size = new Size(300, 20)
        };

        submitButton = new Button
        {
            Text = "Enviar",
            Size = new Size(80, 30)
        };

        submitButton.Click += (o, e) =>
        {
            DrawForm();
        };

        clearButton = new Button
        {
            Text = "Limpar",
            Size = new Size(80, 30)
        };

        clearButton.Click += (o, e) =>
        {
            ClearCanvas();
        };

        assinatura = new Label
        {
            Text = "Assinatura:",
            AutoSize = true
        };

        signaturePictureBox = new PictureBox
        {
            Size = new Size(500, 400),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        signaturePictureBox.MouseDown += SignaturePictureBox_MouseDown;
        signaturePictureBox.MouseMove += SignaturePictureBox_MouseMove;
        signaturePictureBox.MouseUp += SignaturePictureBox_MouseUp;
        signaturePictureBox.Paint += SignaturePictureBox_Paint;

        pen = new Pen(Color.Black, 5);

        previousPoint = Point.Empty;
    }

    private void ClearCanvas()
    {
        g = Graphics.FromImage(canvasImage);
        g.Clear(Color.White);
        signaturePictureBox.Invalidate(); // Refresh the PictureBox
    }

    private void SetupForm()
    {
        this.Text = "Registro";
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.Black;

        int startY = 230;
        int spacingY = 30;
        int labelWidth = 100;
        int textBoxWidth = 300;
        int buttonWidth = 80;
        int signatureHeight = 100;
        int Width = Screen.GetWorkingArea(this).Width;

        nameLabel.Location = new Point((Width - labelWidth - textBoxWidth) / 2, startY);
        nome.Location = new Point(nameLabel.Right, startY);
        nameLabel.ForeColor = Color.White;
        nome.BackColor = Color.White;

        assinatura.Location = new Point((Width - labelWidth - textBoxWidth) / 2, nome.Bottom + spacingY);
        signaturePictureBox.Location = new Point(assinatura.Right, nome.Bottom + spacingY);
        assinatura.ForeColor = Color.White;
        signaturePictureBox.BackColor = Color.White;

        submitButton.Location = new Point((Width - buttonWidth) / 2, signaturePictureBox.Bottom + spacingY);
        submitButton.BackColor = Color.White;
        submitButton.ForeColor = Color.Black;

        clearButton.Location = new Point((Width - buttonWidth) / 2, submitButton.Bottom + spacingY);
        clearButton.BackColor = Color.White;
        clearButton.ForeColor = Color.Black;

        Controls.Add(nameLabel);
        Controls.Add(nome);
        Controls.Add(submitButton);
        Controls.Add(assinatura);
        Controls.Add(signaturePictureBox);
    }

    private void SignaturePictureBox_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            previousPoint = e.Location;
        }
    }

    private void SignaturePictureBox_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            g = Graphics.FromImage(canvasImage);
            g.DrawLine(pen, previousPoint, e.Location);
            previousPoint = e.Location;
            signaturePictureBox.Invalidate(); // Refresh the PictureBox
        }
    }

    private void SignaturePictureBox_MouseUp(object sender, MouseEventArgs e)
    {
        if (g != null)
        {
            g.Dispose();
            g = null;
        }
    }

    private void SignaturePictureBox_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.DrawImage(canvasImage, Point.Empty);
    }

    private void DrawForm()
    {
        string nomeDigitado = nome.Text.Trim();
        if (string.IsNullOrWhiteSpace(nomeDigitado) || canvasImage == null)
        {
            MessageBox.Show("Por favor, preencha todos os campos antes de salvar.", "Campos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string filePath = "./pastaPessoa/nomeDigitado.png";

        try
        {
            signaturePictureBox .Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            MessageBox.Show("Dados salvos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar a imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Draw drawForm = new Draw();
        drawForm.Show();
        this.Hide();
    }

}
