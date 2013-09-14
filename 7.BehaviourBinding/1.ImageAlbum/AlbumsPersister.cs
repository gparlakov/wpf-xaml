using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImageAlbum
{
    public class AlbumsPersister
    {
        public static IEnumerable<AlbumViewModel> GetAlbums(string xmlPath) 
        {
            var xDocAlbums = XDocument.Load(xmlPath);
            var root = xDocAlbums.Root;
            var albums = root.Elements("album").AsQueryable().Select(AlbumViewModel.FromXmlElement);

            return albums;
        }
    }
}
