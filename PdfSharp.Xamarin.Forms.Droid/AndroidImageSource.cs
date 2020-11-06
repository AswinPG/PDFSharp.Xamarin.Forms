using System;
using System.IO;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using static Android.Graphics.Bitmap;
namespace PdfSharp.Xamarin.Forms.Droid
{
	public class AndroidImageSource : ImageSource
	{
		protected override IImageSource FromBinaryImpl(string name, Func<byte[]> imageSource, int? quality = 75)
		{
			return new AndroidImageSourceImpl(name, () => { return new MemoryStream(imageSource.Invoke()); }, (int)quality);
		}

		protected override IImageSource FromFileImpl(string path, int? quality = 75)
		{
			//if (path.Contains("."))
			//{
			//	string[] tokens = path.Split('.');
			//	tokens = tokens.Take(tokens.Length - 1).ToArray();
			//	path = String.Join(".", tokens);
			//	//path = path.ToLower();
			//}
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InPreferredConfig = Bitmap.Config.Argb8888;
			Bitmap bitmap = BitmapFactory.DecodeFile(path, options);
			//selected_photo.setImageBitmap(bitmap);
			//var res = Android.App.Application.Context.Resources;
			//var resId = res.GetIdentifier(path, "drawable", Android.App.Application.Context.PackageName);
			Stream stream = new MemoryStream();
			
			//Stream stream = new MemoryStream();
			Bitmap drawable = null;

			if (true)
			{
				drawable = resizeAndRotate(bitmap, bitmap.Width, bitmap.Height);
				if (drawable != null)
				{
					drawable.Compress(CompressFormat.Jpeg, quality ?? 75, stream);
				}
			}

			return new AndroidImageSourceImpl(path, () => stream, quality ?? 75) { Bitmap = drawable };
		}

		public Bitmap resizeAndRotate(Bitmap image, int width, int height)
		{
			var matrix = new Matrix();
			var scaleWidth = ((float)width) / image.Width;
			var scaleHeight = ((float)height) / image.Height;
			matrix.PostRotate(90);
			matrix.PreScale(scaleWidth, scaleHeight);
			return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
		}

		protected override IImageSource FromStreamImpl(string name, Func<Stream> imageStream, int? quality = 75)
		{
			return new AndroidImageSourceImpl(name, imageStream, (int)quality);
		}
	}
}