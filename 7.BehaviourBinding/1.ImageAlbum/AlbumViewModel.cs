using ImageAlbum.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace ImageAlbum
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        private ICommand prevCommand;
        private ICommand nextCommand;

        public ICommand Prev
        {
            get
            {
                if (this.prevCommand == null)
                {
                    this.prevCommand = new RelayCommand(
                        (obj) =>
                        {
                            this.MoveToPreviousImage();
                            //MessageBox.Show(obj.ToString());
                        },
                        (obj) =>
                        {
                            return true;
                        });
                }

                return this.prevCommand;
            }
        }

        public ICommand Next 
        { 
            get 
            {
                if (this.nextCommand == null)
                {
                    this.nextCommand = new RelayCommand(
                        (obj) =>
                        {
                            this.MoveToNextImage();
                        },
                        (obj) =>
                        {
                            return true;
                        });
                }

                return nextCommand;
            } 
        }

        public string Name { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public ImageViewModel CurrentImage 
        {
            get
            {
                var images = CollectionViewSource.GetDefaultView(this.Images);
                return images.CurrentItem as ImageViewModel;
            }
        }

        private void MoveToNextImage()
        {
            var images = CollectionViewSource.GetDefaultView(this.Images);
            images.MoveCurrentToNext();
            if (images.IsCurrentAfterLast)
            {
                images.MoveCurrentToFirst();
            }

            PropertyChanged(this, new PropertyChangedEventArgs("CurrentImage"));
        }

        private void MoveToPreviousImage()
        {
            var images = CollectionViewSource.GetDefaultView(this.Images);
            images.MoveCurrentToPrevious();
            if (images.IsCurrentBeforeFirst)
            {
                images.MoveCurrentToLast();
            }

            PropertyChanged(this, new PropertyChangedEventArgs("CurrentImage"));
        }

        public static Expression<Func<XElement, AlbumViewModel>> FromXmlElement
        {
            get
            {
                return element =>
                    new AlbumViewModel
                    {
                        Name = element.Element("name").Value,
                        Images = element.Element("images").Elements("image")
                            .AsQueryable().Select(ImageViewModel.FromXElement)
                    };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
