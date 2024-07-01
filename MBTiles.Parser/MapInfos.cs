using Microsoft.Data.Sqlite;
using System.Data;

namespace MBTiles.Parser
{
    public class MapInfos
    {
        private readonly string dbpath;

        public string Name { get; }
        public int Version { get; }
        public string Attribution { get; }
        public string Description { get; }
        public string Type { get; }
        public int MaxZoom { get; }
        public int MinZoom { get; }
        public string Format { get; }
        public string Bounds { get; }
        public string Center { get; }

        public Dictionary<string, string> Metadatas { get; }

        public static MapInfos FromFile(string path)
        {
            return new MapInfos(path);
        }

        private MapInfos(string dbpath)
        {
            Metadatas = ExtractMetadata(dbpath);

            if (!Metadatas.ContainsKey("name"))
                throw new Exception("Metadata MUST contain 'name' row");
            this.Name = Metadatas["name"];
            if (!Metadatas.ContainsKey("format"))
                throw new Exception("Metadata MUST contain 'format' row");
            this.Format = Metadatas["format"];
            if (this.Format == "pbf")
            {
                if (!Metadatas.ContainsKey("json"))
                    throw new Exception("Metadata MUST contain 'json' row if 'format' is pbf");

            }

            if (Metadatas.ContainsKey("bounds"))
                this.Bounds = Metadatas["bounds"];
            if (Metadatas.ContainsKey("center"))
                this.Center = Metadatas["center"];
            if (Metadatas.ContainsKey("minzoom"))
                this.MinZoom = int.Parse(Metadatas["minzoom"]);
            if (Metadatas.ContainsKey("maxzoom"))
                this.MaxZoom = int.Parse(Metadatas["maxzoom"]);

            if (Metadatas.ContainsKey("attribution"))
                this.Attribution = Metadatas["attribution"];
            if (Metadatas.ContainsKey("description"))
                this.Description = Metadatas["description"];
            if (Metadatas.ContainsKey("type"))
                this.Type = Metadatas["type"];
            if (Metadatas.ContainsKey("version"))
                this.Version = int.Parse(Metadatas["version"]);
            this.dbpath = dbpath;
        }

        public string GetTile(int row, int column, int zoomLevel)
        {
            //zoomLevel--;
            int rowbis = (int)((Math.Pow(2, zoomLevel) - 1 - row));
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string selectQuery = $"Select tile_data from tiles where tile_row={row} AND tile_column = {column} AND zoom_level = {zoomLevel}";
                SqliteCommand selectCommand = new SqliteCommand(selectQuery, db);
                var reader = selectCommand.ExecuteReader();

                var canRead = reader.Read();
                if (canRead)
                {
                    SqliteBlob data = (SqliteBlob)reader.GetStream(0);
                }

            }
            return "";
        }

        private Dictionary<string, string> ExtractMetadata(string dbpath)
        {
            Dictionary<string, string> Metadatas = new Dictionary<string, string>();
            using (SqliteConnection db =
        new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String selectQuery = "SELECT name, value from metadata";

                SqliteCommand slectCommand = new SqliteCommand(selectQuery, db);

                var reader = slectCommand.ExecuteReader();

                while (reader.Read())
                {
                    string metadaName = reader.GetString(0);
                    string value = reader.GetString(1);
                    Metadatas[metadaName] = value;
                }
            }

            return Metadatas;
        }
    }
}