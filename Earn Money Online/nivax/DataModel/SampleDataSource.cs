using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "If you’d claimed it was possible for the average guy on the street to make millions of dollars online just a decade ago, the chances are good you’d have heard nothing but laughter in response.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Sell Stock Photography",
                    "The Internet has caused an explosion in the demand for stock photography. As the number of media and commercial outlets has increased dramatically with the growth of online media there’s an enormous need for high quality stock photos.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nThe Internet has caused an explosion in the demand for stock photography. As the number of media and commercial outlets has increased dramatically with the growth of online media there’s an enormous need for high quality stock photos.\n\nYuri Arcurs is the man everyone turns to for their stock photo needs. As the world’s top selling stock photographer he sells an image every 8 seconds, 24 hours a day (that works out to over 4 million images each year). Arcurs makes millions of dollars each year simply by being the best at what he does. There are lots of great photographers in the world and the barrier to entry is as low as ownership of a camera, but Arcurs has managed to build a reputation online for consistent, high quality and imaginative images. If you’re a professional photographer (or even just a hobbyist) you should consider the possibilities of selling stock images online.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Sell Stock Photography", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Earn Money Online" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Tweet For Sponsors",
                     "SponsoredTweets.com is an online platform that allows you to make money on Twitter by charging sponsors for communicating their advertising messages to your followers.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nSponsoredTweets.com is an online platform that allows you to make money on Twitter by charging sponsors for communicating their advertising messages to your followers. You set the amount you want to get paid for every tweet you make, choose a category and select keywords you want to work with. You then wait for advertisers to contact you and take you up on your offer, paying you the amount you specified for each tweet that you make. \n\nAll throughout the process, the tweeter has full control over his or her account, and may choose the wordings of the tweets, or may choose to reject the tweet altogether.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Tweet For Sponsors", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Blog for Ad Revenues",
                     "If, however, you already have a blog with a devoted following it should be easy for you to leverage your readers into hard cash. Ad networks such as Google AdSense pay big money to place their ads on your site, and you’ll receive a payment every time a reader clicks one.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf, however, you already have a blog with a devoted following it should be easy for you to leverage your readers into hard cash. Ad networks such as Google AdSense pay big money to place their ads on your site, and you’ll receive a payment every time a reader clicks one. While it’s easy to go overboard and fill every spare pixel, if you place your ads well it’s possible to make a comfortable income from your site. One of the most successful bloggers around today is John Chow, a Canadian blogger who makes more than $40,000 a month through ad sales and other revenue streams. Ironically, his blog is about ways to make money online.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Blog for Ad Revenues", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Sell Affiliate Products",
                     "If you have a flair for sales copy you could try your hand at selling products for affiliates. While many people take the seedy route of selling diet pills and penis enlargement products.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf you have a flair for sales copy you could try your hand at selling products for affiliates. While many people take the seedy route of selling diet pills and penis enlargement products, if you want to keep your conscience clear you’ll find that Amazon runs a very successful affiliate program that allows you to make money advertising any of the products for sale on their site. eBay also have a pretty good affiliate setup, with their top affiliates earning $1.3 Million a month, WOW!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Sell Affiliate Products", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Write an e Book",
                     "In recent years the self-publishing world has exploded online to the point at which you don’t even have to run your own site in order to promote a book. Amazon, Barnes & Noble and Kobo are just a few of the sites on which you could self-publish today, with commission rates of around 70% available on every sale.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIn recent years the self-publishing world has exploded online to the point at which you don’t even have to run your own site in order to promote a book. Amazon, Barnes & Noble and Kobo are just a few of the sites on which you could self-publish today, with commission rates of around 70% available on every sale. The number of eBooks on Amazon reached 8 million last year, and Amazon stated that eBooks are outselling hard backs 2 to 1, 62% of ebook sales fell into the Thriller and Mystery genre, so if you feel that you could pull this style of genre off then you will be in for a good chance of sales. \n\n26 year old self published author, Amanda Hocking from Minnesota makes more than $2 Million a year from her ebook sales. Amanda Hocking’s stories about, trolls, vampires and zombies and  ‘supernatural teen romances’ sell for $2.99 or for as little as $.99.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Write an e Book", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Become a Virtual Assistant",
                     "Every small businessman would love to hire a full time assistant to take care of the little things, but many simply can’t afford one.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nEvery small businessman would love to hire a full time assistant to take care of the little things, but many simply can’t afford one. Thanks to the Internet, though, they can now hire part time assistants who work for a whole host of clients, and all at a much lower cost than a full time staff member. If you work from home this may be a perfect opportunity to make a consistent income. Virtual assistants can earn $20 an hour in return for booking travel tickets, interacting with clients and dealing with the daily needs of small businesses.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Become a Virtual Assistant", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Lease Your Skills",
                     "Most people have at least one skill that carries a market value, though until now that skill may have been impossible to monetize in the traditional job market.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nMost people have at least one skill that carries a market value, though until now that skill may have been impossible to monetize in the traditional job market. Sites such as 99Designs, Elance, Freelancer and iWriter allow people to hire out their skills as writers, coders, designers, translators and lots more on a contract basis. Freelancing sites are a great way to boost your income in your free time, and with enough motivation and hard work you could find yourself earning a comfortable full time income from them.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Lease Your Skills", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Selling on eBay",
                     "eBay is a great way to turn your unwanted things into a little spending money, but it isn’t just a place to sell your old Star Wars action figures.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\neBay is a great way to turn your unwanted things into a little spending money, but it isn’t just a place to sell your old Star Wars action figures. In fact, eBay’s global marketplace can offer a great way for canny traders to buy and sell their way to profit. By buying wholesale you can sell anything with a mark up. Even better, if you have the skills to make things people want to buy you could start your own home-based craft business, selling to customers around the world. Matt & Amanda Clarkson are a successful couple who so far have managed to make over $8 Million in eBay sales.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Selling on eBay", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Become A Mobile App Tester",
                     "People that are rather uncertain of their application development potential can still make some money through the usage of iPhone apps.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nPeople that are rather uncertain of their application development potential can still make some money through the usage of iPhone apps. People that have the time and desire to test iPhone apps and discover bugs can be rewarded payments for their efforts. uTest is one such application. Individuals that have signed up will also build some reputation on the basis of the testing they have done so far. Better reputation signifies access to more profitable app testing opportunities.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Become A Mobile App Tester", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Earn Money Online" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Designing T-Shirts",
                     "Finally, if you have something of an artistic streak you could kick off the next viral sensation with your own range of funky, arty t-shirts.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nFinally, if you have something of an artistic streak you could kick off the next viral sensation with your own range of funky, arty t-shirts. Sites such as CafePress allow users to upload their own t-shirt designs and sell them on their personal online store. You can also contact distributors such as threadless.com or designbyhumans.com to release your t-shirt designs to the masses. If your designs catch the eye you could be looking at enormous profits when they take off in a big way. Checkout our article The Top 10 Tips On Starting Your Own Successful Clothing Line, this should give you an idea on how to kick things off.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Designing T-Shirts", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Earn Money Online" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
