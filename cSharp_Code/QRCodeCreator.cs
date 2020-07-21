using System;
using System.Drawing;
using System.Drawing.Imaging;

public partial class QR_Code: System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {

		try {
			CreateQRCode(Request.QueryString["code"]);
		}
		catch {
			CreateQRCode("No Code");
		}

	}

	public System.Drawing.Image CreateQRCode(string inputString) {
		QRCode4CS.QRCode qrcode = new QRCode4CS.QRCode(new QRCode4CS.Options(inputString));
		qrcode.Make();
		System.Drawing.Image canvas = new Bitmap(86, 86);
		Graphics artist = Graphics.FromImage(canvas);
		artist.Clear(Color.White);
		for (int row = 0; row < qrcode.GetModuleCount(); row++) {
			for (int col = 0; col < qrcode.GetModuleCount(); col++) {
				bool isDark = qrcode.IsDark(row, col);

				if (isDark == true) {
					artist.FillRectangle(Brushes.Black, 2 * row + 10, 2 * col + 10, 2 * row + 15, 2 * col + 15);
				}
				else {
					artist.FillRectangle(Brushes.White, 2 * row + 10, 2 * col + 10, 2 * row + 15, 2 * col + 15);
				}
			}
		}
		artist.FillRectangle(Brushes.White, 0, 76, 86, 86);
		artist.FillRectangle(Brushes.White, 76, 0, 86, 86);

		using(var ms = new System.IO.MemoryStream()) {

			canvas.Save(ms, ImageFormat.Png);
			Response.ClearContent();
			Response.Clear();
			Response.Buffer = true;
			ms.WriteTo(Response.OutputStream);
			Response.ContentType = "image/png";
			Response.Flush();
		}

		artist.Dispose();
		return canvas;
	}
}