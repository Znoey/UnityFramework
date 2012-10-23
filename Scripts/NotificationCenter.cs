using UnityEngine;
using System.Collections;


namespace NotificationCenter
{
	/// <summary>
	/// Notification callback when adding an observer.
	/// </summary>
	public delegate void NotificationCallback(AbstractNotification note);

	/// <summary>
	/// Static class to use as the observer pattern.  Holds a collection of notification types 
	/// and each type contains a series of listeners as well as the function to call when the notification is posted.
	/// </summary>
	public static class Notify
	{

		/// <summary>
		/// The notifications collection as a jagged list with observers & callbacks.
		/// </summary>
		public static Hashtable m_Notifications = new Hashtable();

		/// <summary>
		/// Adds the observer.
		/// </summary>
		/// <param name='observer'>
		/// Observer to add to this notification type.
		/// </param>
		/// <param name='callBack'>
		/// Call back for what this type of notification is posted.
		/// </param>
		/// <typeparam name='NoteType'>
		/// The type of notification we're listening to with this observer.
		/// </typeparam>
		public static void AddObserver<NotificationType> (object observer, NotificationCallback callBack) where NotificationType : AbstractNotification 
		{
			if(callBack == null || callBack.Equals(null) )
			{
				Debug.LogError("Notify.AddObserver cannot have a null callback");
				return;
			}
			if( observer == null || observer.Equals(null))
			{
				Debug.LogError("Notify.AddObserver cannot have a null object");
				return;
			}

			if (!m_Notifications.ContainsKey (typeof(NotificationType))) {
				m_Notifications.Add(typeof(NotificationType), new Hashtable());
			}
			var table = m_Notifications[typeof(NotificationType)] as Hashtable;
			if( !table.ContainsKey(typeof(NotificationType)) )
			{
				table.Add(observer, callBack);
			}
			else{
				Debug.LogError("Notify.AddObserver already contains this observer on notifications " + typeof(NotificationType).Name + ".");
			}
		}

		/// <summary>
		/// Removes the observer from the supplied type of notifications.
		/// </summary>
		/// <param name='observer'>
		/// Observer to remove from the notification list.
		/// </param>
		/// <typeparam name='NoteType'>
		/// The type of notification we're removing this observer from.
		/// </typeparam>
		public static void RemoveObserver<NotificationType>(object observer) where NotificationType : AbstractNotification
		{
			if( m_Notifications.ContainsKey(typeof(NotificationType)) && observer != null )
			{
				var table = m_Notifications[typeof(NotificationType)] as Hashtable;
				if( table.ContainsKey(observer) )
				{
					table.Remove(observer);
				}
			}
		}

		/// <summary>
		/// Removes the observer from any notifications it was listening for.
		/// </summary>
		/// <param name='observer'>
		/// Observer to remove from any lists.
		/// </param>
		public static void RemoveObserver(object observer)
		{			
			foreach(AbstractNotification note in m_Notifications.Keys)
			{
				var table = m_Notifications[note] as Hashtable;
				if( table.ContainsKey(observer))
				{
					table.Remove(observer);
				}
			}
		}


		/// <summary>
		/// Post the specified note to anyone listening.
		/// </summary>
		/// <param name='note'>
		/// Note to post, usually a subclassed version of the specified notification type.
		/// </param>
		public static void Post(AbstractNotification note)
		{
			if( note == null ) {
				Debug.LogError("Notify.Post cannot post a null notification");
				return;
			}

			if (m_Notifications.ContainsKey(note.GetType())) {
				var table = m_Notifications[note.GetType()] as Hashtable;
				foreach (NotificationCallback item in table.Values){
					item(note);
				}
			}
		}
	}

	public abstract class AbstractNotification
	{
		public object Data {get; set;}
		public object Sender {get; set;}
		public AbstractNotification (object _Sender, object _Data)
		{
				Data = _Data;
				Sender = _Sender;
		}
	}

	public class Notification<DataType> : AbstractNotification
	{
		public new DataType Data { get; set; }
		public Notification (object _Sender, DataType _Data) : base (_Sender, _Data) {}
	}
}

