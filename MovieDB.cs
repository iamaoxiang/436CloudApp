using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Query
{
    internal class MovieDB
    {
        private static AzureTable table = new AzureTable("MovieTable");
        private static AzureBlob blob = new AzureBlob("movieblob");
        private string IMBDQueryFormat = "http://www.omdbapi.com/?apikey=673bb96e&t={0}";

        private Stream GetStream(string uri)
        {
            WebRequest req = null;
            try
            {
                req = System.Net.WebRequest.Create(uri);
                return req.GetResponse().GetResponseStream();
            }
            catch (Exception)
            {
                // catch all errors
                return null;
            }
        }

        public MovieEntity Query(string title)
        {
            // lookup in table
            List<MovieEntity> lookupList = table.Query(title);
            MovieEntity entity = lookupList.Count >= 1 ? lookupList[0] : null;

            // if no result, lookup on IMDB
            if (entity == null)
            { 
                // load the file as a stream
                string requestString = string.Format(IMBDQueryFormat, title);
                Uri requestUri = new Uri(requestString);

                using (Stream stream = GetStream(requestUri.ToString()))
                {
                    StreamReader sr = new StreamReader(stream);
                    entity = new MovieEntity(title, sr.ReadToEnd());
                }

                // now copy the poster into our own blob
                if (!string.IsNullOrEmpty(entity.PosterUrl))
                {
                    using (Stream stream = GetStream(entity.PosterUrl))
                    {
                        Uri path = new Uri(entity.PosterUrl);
                        entity.PosterUrl = blob.UploadToContainer(stream, Path.GetFileName(path.LocalPath));
                    }
                }

                table.Insert(entity);
            }

            // return result
            return entity;
        }

    }
}