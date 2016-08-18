using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows;
using System.Xml;
using System.Xml.XPath;
using ViewPortTools;


namespace KneeInnovation3D.EntityTools
{
	public class PointCollections
	{
  
		//public static void Transform(Point3DCollection Points, Transform3D transform)
		//{
		//    for (int i = 0; i < Points.Count; i++)
		//    {
		//        Points[i] = transform.Transform(Points[i]);
		//    }
		//}

		//public static void InverseTransform(Point3DCollection Points, Transform3D transform)
		//{
		//    for (int i = 0; i < Points.Count; i++)
		//    {
		//        Points[i] = transform.Inverse.Transform(Points[i]);
		//    }
		//}

		//public static void Scale(Point3DCollection Points, double X, double Y, double Z)
		//{
		//    for (int i = 0; i < Points.Count; i++)
		//    {
		//        Points[i] = new Point3D(Points[i].X * X, Points[i].Y * Y, Points[i].Z * Z);
		//    }
		//}

		public static Rect3D Extents(Point3DCollection points)
		{
			double minX = 9e99;
			double maxX = -9e99;
			double minY = 9e99;
			double maxY = -9e99;
			double minZ = 9e99;
			double maxZ = -9e99;
			foreach (Point3D p in points)
			{
				if (p.X < minX) minX = p.X;
				if (p.Y < minY) minY = p.Y;
				if (p.Z < minZ) minZ = p.Z;
				if (p.X > maxX) maxX = p.X;
				if (p.Y > maxY) maxY = p.Y;
				if (p.Z > maxZ) maxZ = p.Z;
			}
			return new Rect3D(minX, minY, minZ, maxX - minX, maxY - minY, maxZ - minZ);
		}

		public static Rect3D Extents(List<Point3DCollection > points)
		{
			Point3DCollection combined = new Point3DCollection();

			foreach (var v in points)
			{ 
				foreach (var p in v)
				{
					combined.Add(p);
				}
			}

		  return  Extents(combined);

		}

		public static Rect3D Extents(List<Point3DCollection []> points)
		{
			Point3DCollection combined = new Point3DCollection();

			foreach (var v in points)
			{
				foreach (var p in v)
				{
					foreach (var t in p)
					{
						combined.Add(t);
					}
				}
			}

			return Extents(combined);

		}
		public static Point3D GetCenter(Point3DCollection points)
		{
			Rect3D bounds = Extents(points);
			return new Point3D(bounds.X + (bounds.SizeX / 2), bounds.Y + (bounds.SizeY / 2), bounds.Z + (bounds.SizeZ / 2));
		}
 
		public static List<Point3DCollection> ReadDelcamPicFile(string filename)
		{
			List<Point3DCollection> polys = new List<Point3DCollection> { };
			Point3DCollection allPoints = new Point3DCollection(1000);
			List<int> numPointsPerSegment = new List<int> { };
			try
			{
				Regex segmentsRx = new Regex(@"^\s+(\d+)\s+(\d+)$");
				Regex coordsRx = new Regex(@"^\s+([0-9.-]+E[+-][0-9]+)\s+([0-9.-]+E[+-][0-9]+)\s+([0-9.-]+E[+-][0-9]+)$");
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader s = new StreamReader(fs))
					{
						while (!s.EndOfStream)
						{
							string line = s.ReadLine().TrimEnd();
							Match matchSeg = segmentsRx.Match(line);
							if (matchSeg.Success)
							{
								numPointsPerSegment.Add(int.Parse(matchSeg.Groups[2].Value));
							}
							Match matchCoord = coordsRx.Match(line);
							if (matchCoord.Success)
							{
								allPoints.Add(new Point3D(double.Parse(matchCoord.Groups[1].Value),
														  double.Parse(matchCoord.Groups[2].Value),
														  double.Parse(matchCoord.Groups[3].Value)));
							}
						}
					}
				}
				int curOffset = 0;
				for (int i = 0; i < numPointsPerSegment.Count; i++)
				{
					Point3DCollection pPts = new Point3DCollection(1000);
					for (int p = curOffset; p < (curOffset + numPointsPerSegment[i]); p++)
					{
						pPts.Add(allPoints[p]);
					}
					polys.Add(pPts);
					curOffset += numPointsPerSegment[i];
				}
				return polys;
			}
			catch
			{
				throw;
			}
			finally
			{

			}

		}
   
		public static void WriteDelcamPicFile(List<Point3DCollection> polylines, string filename)
		{
			try
			{
				if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
				using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
				{
					using (System.IO.StreamWriter tw = new System.IO.StreamWriter(fs))
					{
						tw.Write(" \r\n \r\n *\r\n");

						int totPix = 0;
						for (int i = 0; i < polylines.Count; i++)
						{
							totPix += polylines[i].Count;
						}
						string intLine = "";
						if (totPix > 1000000)
						{
							intLine += string.Format(" {0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
													 GetField(4, 6),
													 GetField(3, 6),
													 GetField(-1, 6),
													 GetField(polylines.Count, 6),
													 GetField(totPix, 11),
													 GetField(-2, 6),
													 GetField(2, 6),
													 GetField(0, 6),
													 GetField(0, 6),
													 GetField(0, 6));
						}
						else
						{
							intLine += string.Format(" {0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
													 GetField(4, 6),
													 GetField(3, 6),
													 GetField(-1, 6),
													 GetField(polylines.Count, 6),
													 GetField(totPix, 6),
													 GetField(-2, 6),
													 GetField(2, 6),
													 GetField(0, 6),
													 GetField(0, 6),
													 GetField(0, 6));
						}
						tw.Write(intLine + "\r\n");

						string sizeLine = "";
						sizeLine = string.Format(" {0}{1}{2}{3}{4}{5}{6}",
												 GetField(0, "0.0000", 10),
												 GetField(0, "0.0000", 10),
												 GetField(0, "0.0000", 10),
												 GetField(0, "0.0000", 10),
												 GetField(0, "0.0000", 10),
												 GetField(0, "0.0000", 10),
												 GetField(0, "0.0000", 10));
						tw.Write(sizeLine + "\r\n");

						int ins = GetInstructionCode(1, 0, 0, 0, 0, 0);
						string insCode = "";
						int fieldSz;
						if (totPix > 1000000)
							fieldSz = 11;
						else
							fieldSz = 6;

						for (int i = 0; i < polylines.Count; i++)
						{
							insCode = string.Format(" {0} {1} \r\n", GetField(ins, 11), GetField(polylines[i].Count, fieldSz));
							tw.Write(insCode);
						}

						string ptCode = "";
						for (int i = 0; i < polylines.Count; i++)
						{
							for (int p = 0; p < polylines[i].Count; p++)
							{
								ptCode = string.Format(System.Globalization.CultureInfo.InvariantCulture,
													   " {0,14:E} {1,14:E} {2,14:E}", polylines[i][p].X, polylines[i][p].Y, polylines[i][p].Z);
								tw.Write(ptCode + "\r\n");
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private static int GetInstructionCode(int ipcol, int iarc, int iimrk, int ijmrk, int iupdn, int idash)
		{
			return ((ipcol * 2048) + (iarc * 512) + (iimrk * 64) + (ijmrk * 32) + (iupdn * 8) + (idash * 1));
		}

		private static string GetField(int value, int fieldLength)
		{
			string tmp = value.ToString("0");
			tmp = new string(' ', (fieldLength - tmp.Length)) + tmp;
			return tmp;
		}

		private static string GetField(double value, string formatCode, int fieldLength)
		{
			string tmp = value.ToString(formatCode, System.Globalization.CultureInfo.InvariantCulture);
			tmp = new string(' ', (fieldLength - tmp.Length)) + tmp;
			return tmp;
		}

		public static void BadoiuClarksonIteration(List<Point3D >  set, int iter,out Point3D  cen, out double rad )
		{
			// Choose any point of the set as the initial
			// circumcenter

			cen = set[0];
			rad = 0;

			for (int i = 0; i < iter; i++)
			{

				int winner = 0;

				double distmax = (new Point3D ( cen.X, cen.Y, 0) - new  Point3D(set[0].X, set[0].Y, 0)).Length;

				// Maximum distance point

				for (int j = 1; j < set.Count; j++)
				{

					double dist = (new Point3D(cen.X, cen.Y, 0) - new Point3D(set[j].X, set[j].Y, 0)).Length;

					if (dist > distmax)
					{

						winner = j;

						distmax = dist;

					}

				}

				rad = distmax;

				// Update

				cen = new Point3D ( cen.X + (1.0 / (i + 1.0)) * (set[winner].X - cen.X), cen.Y + (1.0 / (i + 1.0)) * (set[winner].Y - cen.Y),0 );

			}

		}

		public static bool IsPointInPolygon(Point3DCollection polygon, Point3D point)
		{
			bool isInside = false;
			for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
			{
				if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) && (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
				{ isInside = !isInside; }
			}
			return isInside;
		}

		public static void WriteDatStream(Point3DCollection  polyline, Stream fs)
		{
			try
			{
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
					string header = "PolyFile";
					if (header.Length > 255) header = header = header.Substring (0, 255);
					header += '\0';

					bw.Write(header);
					bw.Write((UInt32)0);
					bw.Write((String)"KICOdatV1");
					bw.Write((UInt32)1);
					bw.Write ((UInt32)1);
					bw.Write((UInt32)polyline.Count);
					for (int i = 0; i < polyline.Count; i++)
					{
						bw.Write(polyline[i].X);
						bw.Write(polyline[i].Y);
						bw.Write(polyline[i].Z);
					}
					bw.Write((UInt16)1000);
				}


			}
			catch
			{

			}

		}

		internal static void WriteMeshGeometryXml(XmlTextWriter x, Point3DCollection  polyline)
		{
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				WriteDatStream (polyline, ms);
				ms.Flush();
				byte[] data = ms.GetBuffer();
				x.WriteBase64(data, 0, data.Length);
			}
		}

		internal static Point3DCollection  GetXmlPolyline(XmlTextReader reader )
		{
			string tempfile = "";

			try
			{
				string currentnode = reader.Name;
				byte[] buffer = new byte[1000];

				tempfile = System.IO.Path.GetTempFileName();
 
				using( System.IO.FileStream fs = new FileStream (tempfile, System.IO.FileMode.Create , System.IO.FileAccess.Write ))
				{
					using (System.IO.BinaryWriter bw = new BinaryWriter(fs))
					{
						do
						{
							int cnt = reader.ReadBase64(buffer, 0, 1000);
							bw.Write(buffer, 0, cnt);
						}
						while (reader.Name == currentnode);


					  

					}
					return FromDatFile(tempfile);
				}
			   
			}
			catch
			{
				return null;
			}
			finally
			{
				if (tempfile != "" && System.IO.File.Exists(tempfile))
					System.IO.File.Delete(tempfile);
			}
		
		}

		public static Point3DCollection FromDatFile(string filename)
		{
			if (!System.IO.File.Exists(filename)) throw new Exception("File not found.");
			BinaryReader br = null;
			try
			{
				br = new BinaryReader(System.IO.File.OpenRead(filename));
				try
				{
					br.BaseStream.Seek(0, SeekOrigin.Begin);


					bool headerCheck = false;
					int headerSize = 0;
					int nextChar;
					while (headerCheck == false)
					{
						nextChar = br.Read();
						if (nextChar == -1) throw new Exception("Invalid header in Poly file");
						else if (nextChar == 0)
						{
							headerSize++;
							headerCheck = true;
						}
						else
						{
							headerSize++;
							if (headerSize == 10) headerCheck = true;
						}
					}

					br.BaseStream.Seek(0, SeekOrigin.Begin);
					br.BaseStream.Position = headerSize;
					int readint = br.ReadInt32();
					string fileVersion = (string)br.ReadString();
					if (fileVersion != "KICOdatV1") throw new Exception("Unsupport Poly File version.");
					int readintdummy1 = br.ReadInt32();
					int readintdummy2 = br.ReadInt32();

					int nodeCount = (int)br.ReadUInt32();

					Point3DCollection points = new Point3DCollection(nodeCount);

					for (int n = 0; n < nodeCount; n++)
					{

						double x = (double)br.ReadDouble();
						double y = (double)br.ReadDouble();
						double z = (double)br.ReadDouble();

						points.Add(new Point3D(x, y, z));
					}

					return points;

				}
				catch (Exception e)
				{
					throw new Exception(string.Format("Error reading Mesh file.\n{0}", e.Message));
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				if (br != null)
				{
					br.Close();
					br = null;
				}
			}
		}

	}
}
