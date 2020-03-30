using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace ORKK.Data
{
    public static class CableChecklistVault
    {
        private static readonly ObservableCollection<CableChecklistObject> CableChecklists = new ObservableCollection<CableChecklistObject>();

        public static int Count => CableChecklists.Count;

        public static CableChecklistObject GetCableChecklist(int id)
        {
            return CableChecklists.FirstOrDefault(x => x.ID == id);
        }

        public static void AddCableChecklist(CableChecklistObject checklist)
        {
            CableChecklists.Add(checklist);
        }

        public static void RemoveCableChecklist(int id)
        {
            CableChecklists.Remove(GetCableChecklist(id));
        }

        public static ObservableCollection<CableChecklistObject> GetCableChecklists()
        {
            return CableChecklists;
        }

        public static void FillVaultFromDB()
        {
            string connString = $@"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename={ System.IO.Path.GetFullPath($@"{System.AppDomain.CurrentDomain.BaseDirectory}..\..\Main.mdf") }";
            using (var conn = new SqlConnection(connString))
            {
                string cableString = @"SELECT * FROM CableChecklistTable";
                using (var command = new SqlCommand(cableString, conn))
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CableChecklistObject cableChecklistObject = new CableChecklistObject(
                            reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3),
                            reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7),
                            reader.GetInt32(8), reader.GetInt32(9));

                        CableChecklists.Add(cableChecklistObject);
                    }
                }
            }
        }
    }

    public enum Damage
    {
        Geen,
        Laag,
        Gemiddeld,
        Hoog,
    }

    public class CableChecklistObject
    {
        public int ID { get; set; }

        public int OrderID { get; set; }

        public int Rupture6D { get; set; }

        public int Rupture30D { get; set; }

        public Damage DamageOutside { get; set; }

        public Damage DamageRustCorrosion { get; set; }

        public int ReducedCableDiameter { get; set; }

        public int PositionMeasuringPoints { get; set; }

        public Damage TotalDamages { get; set; }

        public int TypeDamageRust { get; set; }

        public CableChecklistObject(int id, int orderId, int rupture6D, int rupture30D, int damageOutside, int damageRustCorrosion, int reducedCableDiamater, int positionMeasuringPoints, int totalDamages, int typeDamageRust)
        {
            ID = id;
            OrderID = orderId;
            Rupture6D = rupture6D;
            Rupture30D = rupture30D;
            DamageOutside = (Damage)damageOutside;
            DamageRustCorrosion = (Damage)damageRustCorrosion;
            ReducedCableDiameter = reducedCableDiamater;
            PositionMeasuringPoints = positionMeasuringPoints;
            TotalDamages = (Damage)totalDamages;
            TypeDamageRust = typeDamageRust;
        }
    }
}
