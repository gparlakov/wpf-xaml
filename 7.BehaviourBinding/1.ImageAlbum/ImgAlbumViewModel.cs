using ImageAlbum.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ImageAlbum
{
    public class ImgAlbumViewModel
    {
        public IEnumerable<AlbumViewModel> Albums 
        { 
            get 
            {
                return AlbumsPersister.GetAlbums("..\\..\\albums.xml");
            } 
        }
    }
}
