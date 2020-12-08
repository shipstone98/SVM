using System;
using System.Drawing;
using System.Windows.Forms;

namespace SML_Extensions
{
	public partial class ImageForm : Form
	{
		private const String DefaultSeparator = " - ";
		private const String DefaultText = "Image Viewer";

		public ImageForm()
		{
			this.InitializeComponent();
			this.Text = ImageForm.DefaultText;
		}

		internal ImageForm(LoadedImage image) : this()
		{
			this.Text += ImageForm.DefaultSeparator + image.FileName;
			this.ImagePictureBox.Image = Image.FromStream(image.Stream);
		}
	}
}
