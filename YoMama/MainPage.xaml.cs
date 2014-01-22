using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Common.LoopingDataSource;
using Microsoft.Phone.Tasks;

namespace YoMama
{
	public partial class MainPage
	{
		private List<JokeEntity> _jokes;
		private JokeLoopingSource _ds;

		// Constructor
		public MainPage()
		{
			InitializeComponent();
			SetJokes();
		}
				
		private async void SetJokes()
		{
			_jokes = await ReadFromFile("Jokes.txt");
			_ds = new JokeLoopingSource(_jokes);
			int index = RandomizeItem(0, _jokes.Count - 1);
			_ds.SelectedItem = _jokes[index];
			LoopingSelector.DataSource = _ds;
			LoopingSelector.IsExpanded = true;
		}

		private int RandomizeItem(int min, int max)
		{
			Random rand = new Random();
			return rand.Next(min, max);
		}

		private async Task<List<JokeEntity>> ReadFromFile(string file)
		{
			List<JokeEntity> resultList = new List<JokeEntity>();
			StringBuilder builder = new StringBuilder();
			using (FileStream fileStream = File.OpenRead(file))
			{
				byte[] bytes = new byte[1024*5];

				while ((await fileStream.ReadAsync(bytes, 0, bytes.Length)) != 0)
				{
					builder.Append(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
				}

				string items = builder.ToString();
				string[] rows = items.Split(new[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
				resultList.AddRange(rows.Select((t, i) => new JokeEntity {Index = i, Text = t}));
			}

			return resultList;
		}

		private void SendSmsOnClick(object sender, EventArgs e)
		{
			JokeEntity obj = _ds.SelectedItem as JokeEntity;
			if (obj != null)
			{
				SmsComposeTask smsComposeTask = new SmsComposeTask { Body = obj.Text };
				smsComposeTask.Show();
			}				
		}

		private void ReviewOnClick(object sender, EventArgs e)
		{
			MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
			reviewTask.Show();
		}

		private void FeedbackOnClick(object sender, EventArgs e)
		{
			EmailComposeTask task = new EmailComposeTask
			{
			    To = "acid@klippanlan.net",
			    Subject = "Feedback for Yo Mama on Windows Phone"
			};
		    task.Show();
		}

        //private void RandomJokeOnClick(object sender, EventArgs e)
        //{
        //    int index = RandomizeItem(0, _jokes.Count - 1);
        //    _ds.SetSelectedItem(index);
        //    LoopingSelector.DataSource.SelectedItem = _jokes[index];
        //}
	}
}