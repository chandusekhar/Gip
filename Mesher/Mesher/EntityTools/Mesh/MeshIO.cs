using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Media.Media3D;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization ;


namespace KneeInnovation3D.EntityTools
{
    public class MeshIo
    {
        private const int DefaultBufferSize = 1024;

        #region Read\Write stl

        public  static MeshGeometry3D ReadStl(string path)
        {
            if (!System.IO.File.Exists(path)) return null;

            MeshGeometry3D m = null;

            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    m = ReadStlascii(stream);

                }
            }
            catch
            {
            }

            if (m != null && m.Positions.Count > 0) return m;

 
            try
            {
                using (Stream stream = File.OpenRead(path))
                {
                    using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, true))
                    {
                        m = ReadStlBinary(reader);
                    }
                }
            }
            catch
            {
            }

     

            return m;
        }

        public static MeshGeometry3D ReadStlBinary(BinaryReader reader)
        {
            if (reader == null)
                return null;

            MeshGeometry3D stl = new MeshGeometry3D();

            try
            {
                byte[] buffer = new byte[80];


                //Read (and ignore) the header and number of triangles.
                buffer = reader.ReadBytes(80);
                int numberoftris = (int)reader.ReadInt32();

                stl.Positions = new Point3DCollection();
                stl.TriangleIndices = new System.Windows.Media.Int32Collection();

                int count = 0;

                //Read each facet until the end of the stream. Stop when the end of the stream is reached.
                while ((reader.BaseStream.Position != reader.BaseStream.Length))
                {

                    double ni = reader.ReadSingle();
                    double nj = reader.ReadSingle();
                    double nk = reader.ReadSingle();
                    double x1 = reader.ReadSingle();
                    double y1 = reader.ReadSingle();
                    double z1 = reader.ReadSingle();
                    double x2 = reader.ReadSingle();
                    double y2 = reader.ReadSingle();
                    double z2 = reader.ReadSingle();
                    double x3 = reader.ReadSingle();
                    double y3 = reader.ReadSingle();
                    double z3 = reader.ReadSingle();
                    byte[] boolbuff = reader.ReadBytes(2);


                    stl.Positions.Add(new Point3D(x1, y1, z1));
                    stl.Positions.Add(new Point3D(x2, y2, z2));
                    stl.Positions.Add(new Point3D(x3, y3, z3));
                    stl.TriangleIndices.Add(count);
                    stl.TriangleIndices.Add(count + 1);
                    stl.TriangleIndices.Add(count + 2);
                    count += 3;


                }
            }
            catch (Exception err)
            {
              //  throw new Exception(string.Format("Error reading STL file.\n{0}", err.Message));
            }

            return stl;
        }

        public static MeshGeometry3D ReadStlascii(Stream stream)
        {
            MeshGeometry3D output = new System.Windows.Media.Media3D.MeshGeometry3D(); ;
            var reader = new StreamReader(stream);

            try
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        continue;
                    }

                    line = line.Trim();
                    if (line.Length == 0 || line.StartsWith("\0") || line.StartsWith("#") || line.StartsWith("!")
                        || line.StartsWith("$"))
                    {
                        continue;
                    }

                    string id, values;
                    ParseLine(line, out id, out values);
                    switch (id)
                    {
                        case "solid":
                            _header = values.Trim();
                            break;
                        case "facet":
                            List<Point3D> pts = ReadFace(reader, values);
                            foreach (var v in pts) output.Positions.Add(v);
                            break;
                        case "endsolid":
                            break;
                    }
                }
            }
            catch
            { 
            
            }

            if (output.Positions.Count > 3)
            {
                for (int i = 0; i < output.Positions.Count; i++)
                {
                    output.TriangleIndices.Add(i);
                }
            }

            return output;
        }

        private static List<Point3D> ReadFace(StreamReader reader, string normal)
        {

            List<Point3D> points = new List<Point3D>();
            ReadLine(reader, "outer");
            while (true)
            {
                var line = reader.ReadLine();
                Point3D point;
                if (TryParseVertex(line, out point))
                {
                    points.Add(point);
                    continue;
                }

                string id, values;
                ParseLine(line, out id, out values);

                if (id == "endloop")
                {
                    break;
                }
            }

            ReadLine(reader, "endfacet");


            return points;
        }

        private static void ReadLine(StreamReader reader, string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            var line = reader.ReadLine();
            string id, values;
            ParseLine(line, out id, out values);

            if (!string.Equals(token, id, StringComparison.OrdinalIgnoreCase))
            {
                throw new FileFormatException("Unexpected line.");
            }
        }

        private static void ParseLine(string line, out string id, out string values)
        {
            line = line.Trim();
            int idx = line.IndexOf(' ');
            if (idx == -1)
            {
                id = line;
                values = string.Empty;
            }
            else
            {
                id = line.Substring(0, idx).ToLower();
                values = line.Substring(idx + 1);
            }
        }

        private static bool TryParseVertex(string line, out Point3D point)
        {
            var match = VertexRegex.Match(line);
            if (!match.Success)
            {
                point = new Point3D();
                return false;
            }

            double x = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            double y = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            double z = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            point = new Point3D(x, y, z);
            return true;
        }

        private static readonly Regex VertexRegex = new Regex(@"vertex\s*(\S*)\s*(\S*)\s*(\S*)", RegexOptions.Compiled);

        private static string _header;

        public static bool WriteBinaryStl(Stream stream, MeshGeometry3D mesh)
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                byte[] header = Encoding.ASCII.GetBytes("Binary STL generated by JKH");
                byte[] headerFull = new byte[80];

                Buffer.BlockCopy(header, 0, headerFull, 0, Math.Min(header.Length, headerFull.Length));

                //Write the header and facet count.
                writer.Write(headerFull);
                writer.Write((UInt32)mesh.TriangleIndices.Count / 3);


                for (int i = 0; i < mesh.TriangleIndices.Count; i += 3)
                {
                    if (mesh.Normals != null && mesh.Normals.Count == mesh.TriangleIndices.Count / 3)
                    {
                        writer.Write((float)mesh.Normals[i].X);
                        writer.Write((float)mesh.Normals[i].Y);
                        writer.Write((float)mesh.Normals[i].Z);
                    }
                    else
                    {
                        writer.Write((float)0);
                        writer.Write((float)0);
                        writer.Write((float)1);
                    }
                    int idx1 = i;
                    int idx2 = i + 1;
                    int idx3 = i + 2;
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx1]].X);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx1]].Y);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx1]].Z);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx2]].X);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx2]].Y);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx2]].Z);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx3]].X);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx3]].Y);
                    writer.Write((float)mesh.Positions[mesh.TriangleIndices[idx3]].Z);
                    byte a = 0;
                    byte b = 0;

                    writer.Write(a);
                    writer.Write(b);

                }
            }

            return true;
        }

        public static bool WriteStlFile(MeshGeometry3D mesh, string filename)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    WriteBinaryStl (fs, mesh);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void WriteAsciiStlFile(MeshGeometry3D mesh, string filename)
        {
            using (System.IO.StreamWriter streamfile = new System.IO.StreamWriter(filename))
            {
                streamfile.WriteLine("solid - Simpleware");

                for (int i = 0; i < mesh.Positions.Count  ; i++)
                {
                    Vector3D n = KneeInnovation3D.EntityTools.MeshGeometryFunctions.CalculateNorms(mesh.Positions[i], mesh.Positions[i + 1], mesh.Positions[i + 2]);
                    streamfile.WriteLine(" facet normal " + n.X.ToString() + " " + n.Y.ToString() + " " + n.Z.ToString());
                    streamfile.WriteLine("  outer loop");
                    streamfile.WriteLine("   vertex " + mesh.Positions[i].X.ToString() + " " + mesh.Positions[i].Y.ToString() + " " + mesh.Positions[i].Z.ToString());
                    streamfile.WriteLine("   vertex " + mesh.Positions[i + 1].X.ToString() + " " + mesh.Positions[i + 1].Y.ToString() + " " + mesh.Positions[i + 1].Z.ToString());
                    streamfile.WriteLine("   vertex " + mesh.Positions[i + 2].X.ToString() + " " + mesh.Positions[i + 2].Y.ToString() + " " + mesh.Positions[i + 2].Z.ToString());
                    streamfile.WriteLine("  endloop");
                    streamfile.WriteLine(" endfacet");
                    i++;
                    i++;
                }

                streamfile.WriteLine("endsolid");

                streamfile.Close();
            }
        }

        # endregion

        #region X3D

        internal static void  WriteX3D(MeshGeometry3D mesh, Transform3D trans, string file)
        {

            using (var stream = File.Create(file))
            {
                using (var writer = XmlTextWriter.Create(stream))
                {

                    writer.WriteStartDocument(false);
                    writer.WriteDocType("X3D", "ISO//Web3D//DTD X3D 3.0//EN", "http://www.web3d.org/specifications/x3d-3.1.dtd", null);
                    writer.WriteStartElement("X3D");
                    writer.WriteAttributeString("profile", "Immersive");
                    writer.WriteAttributeString("version", "3.1");
                   // writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema-instance");
                   // writer.WriteAttributeString("xsd:noNamespaceSchemaLocation", "http://www.web3d.org/specifications/x3d-3.1.xsd");

                    writer.WriteStartElement("head");
                    //foreach (var kvp in this.Metadata)
                    //{
                    //    writer.WriteStartElement("meta");
                    //    writer.WriteAttributeString("name", kvp.Key);
                    //    writer.WriteAttributeString("value", kvp.Value);
                    //    writer.WriteEndElement(); // meta
                    //}

                    writer.WriteEndElement(); // head
                    writer.WriteStartElement("Scene");

                    ExportX3D(writer, mesh, trans);

                    writer.WriteEndElement(); // Scene
                    writer.WriteEndElement(); // X3D
                    writer.WriteEndDocument();
                    writer.Close();

                }

            }

        }

        private static  void ExportX3D(XmlWriter writer, MeshGeometry3D model, Transform3D trans)
        {
            var mesh = model.Clone();
            if (mesh == null)
            {
                // Only MeshGeometry3D is supported.
                return;
            }

            var triangleIndices = new StringBuilder();
            int count = 0;
            foreach (int i in mesh.TriangleIndices)
            {
                triangleIndices.Append(i + " ");

                count++;

                if (count == 3)
                {
                    triangleIndices.Append("-1");
                    count = 0;
                }
            }

            var points = new StringBuilder();
            foreach (var pt in mesh.Positions)
            {
                points.AppendFormat(CultureInfo.InvariantCulture, "{0} {1} {2} ", pt.X, pt.Y, pt.Z);
            }

            writer.WriteStartElement("Transform");
            writer.WriteStartElement("Shape");
            writer.WriteStartElement("IndexedFaceSet");
            writer.WriteAttributeString("coordIndex", triangleIndices.ToString());
            writer.WriteStartElement("Coordinate");
            writer.WriteAttributeString("point", points.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement(); // IndexedFaceSet
            writer.WriteStartElement("Appearance");

            writer.WriteStartElement("Material");
            writer.WriteAttributeString("diffuseColor", "0.8 0.8 0.2");
            writer.WriteAttributeString("specularColor", "0.5 0.5 0.5");
            writer.WriteEndElement();
            writer.WriteEndElement(); // Appearance

            writer.WriteEndElement(); // Shape
            writer.WriteEndElement(); // Transform
        }


        #endregion

        #region WriteOBJ

        public static void WriteObj(MeshGeometry3D mesh, string filename)
        {

            if (mesh.Normals == null || mesh.Normals.Count == 0)
            {
                mesh.Normals = KneeInnovation3D.EntityTools.MeshGeometryFunctions.CalculateNormals(mesh);
            }

            using (System.IO.StreamWriter streamfile = new StreamWriter(filename))
            {
                streamfile.WriteLine("# Wavefront OBJ file created by Kico");

                for (int i = 0; i < mesh.Positions.Count; i++)
                {
                    streamfile.WriteLine("v " + Math.Round(mesh.Positions[i].X, 6).ToString() + " " + Math.Round(mesh.Positions[i].Y, 6).ToString() + " " + Math.Round(mesh.Positions[i].Z, 6).ToString());
                }

                for (int i = 0; i < mesh.Normals.Count; i++)
                {
                    streamfile.WriteLine("vn " + Math.Round(mesh.Normals[i].X, 6).ToString() + " " + Math.Round(mesh.Normals[i].Y, 6).ToString() + " " + Math.Round(mesh.Normals[i].Z, 6).ToString());
                }

                for (int i = 0; i < mesh.TriangleIndices.Count; i++)
                {
                    streamfile.WriteLine("f " + (mesh.TriangleIndices[i] + 1).ToString() + " " + (mesh.TriangleIndices[i + 1] + 1).ToString() + " " + (mesh.TriangleIndices[i + 2] + 1).ToString());
                    i++;
                    i++;
                }

                streamfile.WriteLine("# EOF");

                streamfile.Close();
            }
        }

        # endregion 

    }
}
