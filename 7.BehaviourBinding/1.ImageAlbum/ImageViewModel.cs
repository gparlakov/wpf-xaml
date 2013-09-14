using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace ImageAlbum
{
    public class ImageViewModel
    {
        public static Expression<Func<XElement, ImageViewModel>> FromXElement
        {
            get
            {
                return element =>
                new ImageViewModel
                {
                    Title = element.Element("title").Value,
                    ImgSource = element.Element("imgSource").Value
                };
            }
        }

        public string Title { get; set; }

        public string ImgSource { get; set; }
    }
}
