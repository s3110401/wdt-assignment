﻿using PartB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PartB
{
    public partial class CineplexEdit : System.Web.UI.Page
    {
        private static CineplexModel cineplexModel = CineplexModel.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(Request.QueryString["id"]);
                Cineplex movie = cineplexModel.getCineplexByID(id);
                if (Page.IsPostBack) return;
                location.Text = movie.Location;
                shortDescription.Text = movie.ShortDecription;
                longDescription.Text = movie.LongDecription;
                cineplexImage.Text = movie.ImageUrl;
            }
            catch (Exception ex)
            {
                Response.Redirect("Default.aspx");
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (location.Text == "" ||
                    shortDescription.Text == "" ||
                    longDescription.Text == "")
                    throw new Exception("Did not save!");

                if (FileUpload1.HasFile)
                {
                    string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg" };
                    string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                    bool isValidFile = false;
                    for (int i = 0; i < validFileTypes.Length; i++)
                    {
                        if (ext == "." + validFileTypes[i])
                        {
                            isValidFile = true;
                            break;
                        }
                    }

                    if (!isValidFile)
                    {
                        throw new Exception("Did not save!");
                    }

                    FileUpload1.SaveAs(Server.MapPath("~/Content/images/") +
                            FileUpload1.FileName);
                    cineplexImage.Text = FileUpload1.FileName;
                }

                int id = int.Parse(Request.QueryString["id"]);
                Cineplex movie = cineplexModel.getCineplexByID(id);

                Cineplex upMovie = cineplexModel.EditCinplex(movie,
                    location.Text,
                    shortDescription.Text,
                    longDescription.Text,
                    cineplexImage.Text,
                    1);
                Label1.Text = "";
                Response.Redirect("Default.aspx");
            }
            catch (Exception ex)
            {
                Label1.Text = "Did not save!";
                Label1.Attributes.CssStyle.Add("color", "red");
            }
        }
        protected void Remove_Click(object sender, EventArgs e)
        {
            try
            {
                MovieModel movieModel = MovieModel.Instance;
                int id = int.Parse(Request.QueryString["id"]);
                Cineplex cineplex = cineplexModel.getCineplexByID(id);
                cineplexModel.SoftDelete(cineplex);
            }
            catch (Exception ex)
            {
                Label1.Text = ex.StackTrace;
            }
            Response.Redirect("Default.aspx");
        }
    }
}