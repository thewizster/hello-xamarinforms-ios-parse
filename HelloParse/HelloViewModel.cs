using System;
using System.ComponentModel;
using Parse;
using System.Threading.Tasks;

namespace HelloParse
{
	public class HelloViewModel : INotifyPropertyChanged
	{
		public HelloViewModel()
		{
			// initialize the properties
			this.Message = "Contacting extraterrestrials. Please wait...";
			this.IsWorking = true;

			// initialize ParseClient.
			// NOTE: Change sever URL. obviously you can't run parse-server on your device.
			Parse.ParseClient.Initialize(new Parse.ParseClient.Configuration {
				ApplicationId = "myAppId",
				Server = "http://localhost:1337/parse/"
			});

			// Save data to parse-server then update the view model properties
			var result = SaveToParseServerAsync("Hello World!");
			result.ContinueWith((antecedant) => {
				this.Message = antecedant.Result;
				this.IsWorking = false;
			});
		}

#region Parse code
		public async Task<string> SaveToParseServerAsync(string msgToSave)
		{
			// set message to save as the default result string
			string result = msgToSave;

			// create a ParseObject and set the message data
			var world = new ParseObject("World");
			world["message"] = msgToSave;

			// Attempt to save to the parse-server
			try
			{
				await world.SaveAsync();
			}
			catch (Exception ex)
			{
				// set result string as the exception message
				result = ex.Message;
			}

			return result;
		}
#endregion

#region INotifyPropertyChanged Implimentation
		public event PropertyChangedEventHandler PropertyChanged;

		// very basic implimentation
		public void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				this.PropertyChanged(this, e);
			}
		}
#endregion

#region Bindable properties
		private string _message = "";
		public string Message
		{
			get { return _message; }
			set
			{
				_message = value;
				this.OnPropertyChanged("Message");
			}
		}

		private bool _isWorking = false;
		public bool IsWorking
		{
			get { return _isWorking; }
			set
			{
				_isWorking = value;
				this.OnPropertyChanged("IsWorking");
			}
		}
#endregion
	}
}

