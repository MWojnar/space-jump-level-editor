using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceJumpLevelEditor
{
	public static class RectangleExtensions
	{
		public static Rectangle OffsetBy(this Rectangle rect, Vector2 offset)
		{
			return new Rectangle(rect.X + (int)offset.X, rect.Y + (int)offset.Y, rect.Width, rect.Height);
		}

		public static int Area(this Rectangle rect)
		{
			return rect.Width * rect.Height;
		}
	}

	public static class IEnumerableExtensions
	{
		private static Random rnd = new Random();

		public static T RandomElement<T>(this IEnumerable<T> source)
		{
			T[] array = source as T[] ?? source.ToArray();
			return array.Length == 0 ? default : array[rnd.Next(array.Length)];
		}
	}
}
