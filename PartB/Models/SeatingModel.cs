﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartB.Models
{
    public struct Seating
    {
        public int CineplexMovieID { set; get; }
        public string SeatRowColumn { set; get; }
        public string extra { set; get; }
    };
    class SeatingModel
    {
        private static string CONNECTION_STRING =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private const int DID_NOT_FIND_SEATING_INDEX = -1;

        private IList<Seating> seatings = new List<Seating>();
        private static SeatingModel instance;

        private SeatingModel() { }

        public static SeatingModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SeatingModel();
                }
                return instance;
            }
        }

        public IList<Seating> Seatings
        {
            get
            {
                return seatings;
            }
        }
        public Seating getSeats(CineplexMovie cineplexMovie)
        {
            seatings = GetSeating();
            for (int i = 0; i < seatings.Count; i++)
            {
                if (seatings[i].CineplexMovieID.Equals(cineplexMovie.cineplexMovieId))
                    return seatings[i];
            }

            throw new CustomCouldntFindException("Can't find!");
        }
        public IList<Seating> GetSeating()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            using (conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    conn.Open();
                    string sql = "select * from [master].[dbo].[Seating]";
                    cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    seatings = new List<Seating>();
                    while (rdr.Read())
                    {
                        Seating seating = new Seating();
                        seating.CineplexMovieID = (int)rdr[0];
                        seating.SeatRowColumn = (string)rdr[1];
                        seating.extra = (string)rdr[2];

                        seatings.Add(seating);
                    }
                    return seatings;
                }
                catch (Exception ex)
                {
                    return seatings;
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                }
            }
        }
        public Seating AddSeating(int cineplexMovieID, string SeatRowColumn, string extra)
        {
            int seatingIndex = SearchSeatingIndex(cineplexMovieID, SeatRowColumn, extra);
            if (seatingIndex != DID_NOT_FIND_SEATING_INDEX) return seatings[seatingIndex];

            seatingIndex = InsertGetId(cineplexMovieID, SeatRowColumn, extra);
            if (seatingIndex != DID_NOT_FIND_SEATING_INDEX)
                throw new CustomCouldntFindException("Failed to add coming soon movie!");

            Seating seating = new Seating();
            seating.CineplexMovieID = cineplexMovieID;
            seating.SeatRowColumn = SeatRowColumn;
            seating.extra = extra;

            seatings.Add(seating);

            return seating;
        }
        private int InsertGetId(int cineplexMovieID, string SeatRowColumn, string extra)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            using (conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO [master].[dbo].[Seating]" +
                        "(CineplexMovieID,SeatRow,SeatColumn,extra) VALUES" +
                        "(@CineplexMovieID,@SeatRowColumn,@extra)";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@CineplexMovieID", SqlDbType.Int).Value = cineplexMovieID;
                    cmd.Parameters.Add("@SeatRowColumn", SqlDbType.VarChar).Value = SeatRowColumn;
                    cmd.Parameters.Add("@extra", SqlDbType.VarChar).Value = extra;

                    return (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    return DID_NOT_FIND_SEATING_INDEX;
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                }
            }
        }
        public int SearchSeatingIndex(int cineplexMovieID, string SeatRowColumn, string extra)
        {
            for (int i = 0; i < seatings.Count; i++)
            {
                if (seatings[i].CineplexMovieID.Equals(cineplexMovieID) &&
                    seatings[i].SeatRowColumn.Equals(SeatRowColumn) &&
                    seatings[i].extra.Equals(extra))
                    return i;
            }
            return DID_NOT_FIND_SEATING_INDEX;
        }
        public Seating EditSeating(Seating oriSeating, string SeatRowColumn, string extra)
        {
            int seatingIndex = SearchSeatingIndex(
                oriSeating.CineplexMovieID,
                oriSeating.SeatRowColumn,
                oriSeating.extra);
            if (seatingIndex == DID_NOT_FIND_SEATING_INDEX)
                throw new CustomCouldntFindException("Failed to add coming soon movie!");

            Update(oriSeating.CineplexMovieID, SeatRowColumn, extra);

            Seating seating = new Seating();
            seating.CineplexMovieID = oriSeating.CineplexMovieID;
            seating.SeatRowColumn = SeatRowColumn;
            seating.extra = extra;

            seatings[seatingIndex] = seating;

            return seating;
        }
        private void Update(int cineplexMovieID, string SeatRowColumn, string extra)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            using (conn = new SqlConnection(CONNECTION_STRING))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE [master].[dbo].[Seating] SET " +
                        "SeatRowColumn = @SeatRowColumn," +
                        "extra = @extra " +
                        "WHERE CineplexMovieID = @CineplexMovieID";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@SeatRowColumn", SqlDbType.VarChar).Value = SeatRowColumn;
                    cmd.Parameters.Add("@extra", SqlDbType.VarChar).Value = extra;

                    cmd.Parameters.Add("@CineplexMovieID", SqlDbType.Int).Value = cineplexMovieID;

                    cmd.ExecuteNonQuery();
                    return;
                }
                catch (Exception ex)
                {
                    throw new CustomCouldntFindException(ex.StackTrace);
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    if (conn != null)
                    {
                        conn.Dispose();
                    }
                }
            }
        }
    }
}
