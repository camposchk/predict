using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Register : Form
{
    private Label nameLabel;
    private TextBox nome;
    private Button submitButton;
    private PictureBox signaturePictureBox;
    private Label assinatura;
    private Graphics g;
    private Point previousPoint;
    private Pen pen;
    private Button clearButton;
    public Register()
    {
        SetupUI();
        SetupForm();
    }

    private void SetupUI()
    {
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

        pen = new Pen(Color.Black, 2);

        previousPoint = Point.Empty;
    }

    private void ClearCanvas()
    {
        signaturePictureBox.Image = new Bitmap(signaturePictureBox.Width, signaturePictureBox.Height);
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
            g = signaturePictureBox.CreateGraphics();
            g.DrawLine(pen, previousPoint, e.Location);
            previousPoint = e.Location;
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
        if (g != null)
        {
            e.Graphics.DrawLine(pen, previousPoint, previousPoint);
        }
    }

    private void DrawForm()
    {
        //     if (string.IsNullOrWhiteSpace(nome.Text) || signaturePictureBox.Image == null)
        //     {
        //         MessageBox.Show("Por favor, preencha todos os campos antes de salvar.", "Campos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //         return;
        //     }
        //     string nomeDigitado = nome.Text;
        //     Image assinaturaImage = (Image)signaturePictureBox.Image.Clone();

        //     string diretorio = Path.GetDirectoryName(Application.ExecutablePath);
        //     string pastaDados = Path.Combine(diretorio, "Dados");

        //     if (!Directory.Exists(pastaDados))
        //     {
        //         Directory.CreateDirectory(pastaDados);
        //     }

        //     string pathNome = Path.Combine(pastaDados, "Nome.txt");
        //     File.WriteAllText(pathNome, nomeDigitado);

        //     string pathAssinatura = Path.Combine(pastaDados, "Assinatura.png");
        //     assinaturaImage.Save(pathAssinatura, System.Drawing.Imaging.ImageFormat.Png);

        //     MessageBox.Show("Dados salvos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        Draw drawForm = new Draw();
        drawForm.Show();
        this.Hide();
    }
}
