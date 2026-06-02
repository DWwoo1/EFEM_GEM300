using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Define.DefineEnumProject.Mail;

namespace FrameOfSystem3.Functional
{
	class PostOffice
	{
		#region <CONSTRUCTOR>
		private static PostOffice _instance = null;
		public static PostOffice GetInstance()
		{
			if (_instance == null)
			{
				_instance = new PostOffice();
			}
			return _instance;
		}
		#endregion </CONSTRUCTOR>

		#region <FILED>
		List<EN_SUBSCRIBER> _subscriberList = new List<EN_SUBSCRIBER>();
		ConcurrentDictionary<EN_SUBSCRIBER, MailBox> _mailBoxList = new ConcurrentDictionary<EN_SUBSCRIBER, MailBox>();

		const EN_SUBSCRIBER UNKOWN = EN_SUBSCRIBER.Unknown;
		#endregion </FILED>

		#region <PROPERTY>
		public List<EN_SUBSCRIBER> SubscriberList { get { return _subscriberList; } }

		public delegate void DelegateNotificationMailArrive(EN_MAIL title);
		#endregion </PROPERTY>

		#region <INTERFACE>
		public bool RequestSubscribe(EN_SUBSCRIBER name, MailBox.NotificationDelegate notificationFunc = null)
		{
			if (_subscriberList.Contains(name))
				return false;

			_subscriberList.Add(name);
			if (false == _mailBoxList.TryAdd(name, new MailBox(notificationFunc)))
				return false;

			return true;
		}
		public bool RegistrationNotificationFunc(EN_SUBSCRIBER name, MailBox.NotificationDelegate notificationFunc)
		{
			if (false == _mailBoxList.ContainsKey(name))
				return false;


			_mailBoxList[name].RegistrationNotificationFunc(notificationFunc);
			return true;
		}

		public bool SendMail(Mail mail)
		{
			if (false == _subscriberList.Contains(mail.To))
				return false;

			MailBox receiveBox;
			if (false == _mailBoxList.TryGetValue(mail.To, out receiveBox)) return false;

			return receiveBox.InsertMail(mail);
		}
		public bool SendMail(EN_SUBSCRIBER sender, EN_SUBSCRIBER receiver, EN_MAIL title, params object[] content)
		{
			return SendMail(new Mail(sender, receiver, title, content));
		}
		public bool SendMail(EN_SUBSCRIBER receiver, EN_MAIL title, params object[] content)
		{
			return SendMail(new Mail(UNKOWN, receiver, title, content));
		}

		public bool GetMail(EN_SUBSCRIBER name, EN_MAIL title, out List<Mail> result)
		{
			if (false == _mailBoxList.ContainsKey(name))
			{
				result = null;
				return false;
			}

			MailBox receiveBox;
            if (false == _mailBoxList.TryGetValue(name, out receiveBox))
			{
				result = null;
				return false;
			}

			return receiveBox.DrawMail(title, out result);
		}
		public bool GetMail(EN_SUBSCRIBER name, EN_SUBSCRIBER from, EN_MAIL title, out List<Mail> result)
		{
			if (false == _mailBoxList.ContainsKey(name))
			{
				result = null;
				return false;
			}

			MailBox receiveBox;
            if (false == _mailBoxList.TryGetValue(name, out receiveBox))
			{
				result = null;
				return false;
			}

			return receiveBox.DrawMail(from, title, out result);
		}

		public bool CheckMail(EN_SUBSCRIBER name, EN_MAIL title)
		{
			List<Mail> tempMail;
			return GetMail(name, title, out tempMail);
		}
		public bool CheckMail(EN_SUBSCRIBER name, EN_SUBSCRIBER from, EN_MAIL title)
		{
			List<Mail> tempMail;
			return GetMail(name, from, title, out tempMail);
		}

		public bool ContainsMail(EN_SUBSCRIBER name, EN_MAIL title)
		{
			if (false == _mailBoxList.ContainsKey(name))
				return false;

			MailBox receiveBox;
			if (false == _mailBoxList.TryGetValue(name, out receiveBox))
				return false;

			return receiveBox.ContainsMail(title);
		}
		public bool ContainsMail(EN_SUBSCRIBER name, EN_SUBSCRIBER from, EN_MAIL title)
		{
			if (false == _mailBoxList.ContainsKey(name))
				return false;

			MailBox receiveBox;
			if (false == _mailBoxList.TryGetValue(name, out receiveBox))
				return false;

			return receiveBox.ContainsMail(from, title);
		}

		public void EmptyMailBoxAll()
		{
			foreach (EN_SUBSCRIBER name in _subscriberList)
			{
				if (false == _mailBoxList.ContainsKey(name)) continue;

				MailBox mailBox;
				if (false == _mailBoxList.TryGetValue(name, out mailBox)) continue;
				mailBox.EmptyBox();
			}

			foreach (EN_SUBSCRIBER name in _subscriberList)
			{
				if (false == _mailBoxList.ContainsKey(name)) continue;

				SendMail(name, name, Define.DefineEnumProject.Mail.EN_MAIL.INIT_MAIL);
			}
		}
		#endregion <INTERFACE>
	}
	class MailBox
	{
		public delegate void NotificationDelegate(EN_MAIL title);
		ConcurrentDictionary<EN_MAIL, ConcurrentQueue<Mail>> _mailList = new ConcurrentDictionary<EN_MAIL, ConcurrentQueue<Mail>>();
		NotificationDelegate _notificationFunc = null;
		ReaderWriterLockSlim _mailboxLock = new ReaderWriterLockSlim();

		public MailBox(NotificationDelegate notificationFunc)
		{
			RegistrationNotificationFunc(notificationFunc);
		}

		public bool InsertMail(Mail mail)
		{
			_mailboxLock.EnterWriteLock();
			if (false == _mailList.ContainsKey(mail.Title))
			{
				if (false == _mailList.TryAdd(mail.Title, new ConcurrentQueue<Mail>())) return false;
			}

			Mail copyMail = mail.Clone() as Mail;
			_mailList[mail.Title].Enqueue(copyMail);
			_mailboxLock.ExitWriteLock();

			if (_notificationFunc != null)
				_notificationFunc(mail.Title);

			return true;
		}
		public bool DrawMail(EN_MAIL title, out List<Mail> result)
		{
			if (false == _mailList.ContainsKey(title))
			{
				result = null;
				return false;
			}

			result = new List<Mail>();
			Mail mail;
			while(_mailList[title].TryDequeue(out mail))
			{
				result.Add(mail);
			}

			if (_mailList[title].Count <= 0)
			{
				_mailboxLock.EnterWriteLock();
				ConcurrentQueue<Mail> removeList;
				while (false == _mailList.TryRemove(title, out removeList))
				{
					//Thread.Sleep(1);	// 넘겨주면 안됨
				}
				_mailboxLock.ExitWriteLock();
			}

			if (result.Count <= 0)
				return false;

			return true;
		}
		public bool DrawMail(EN_SUBSCRIBER from, EN_MAIL title, out List<Mail> result)
		{
			if (false == _mailList.ContainsKey(title))
			{
				result = null;
				return false;
			}

			List<Mail> deferList = new List<Mail>();
			result = new List<Mail>();
			Mail mail;
			while (_mailList[title].TryDequeue(out mail))
			{
				if (mail.From.Equals(from))
					result.Add(mail);
				else
					deferList.Add(mail);
			}

			foreach(Mail deferMail in deferList)
			{
				_mailList[title].Enqueue(deferMail);
			}

			if(_mailList[title].Count <= 0)
			{
				_mailboxLock.EnterWriteLock();
				ConcurrentQueue<Mail> removeList;
				while (false == _mailList.TryRemove(title, out removeList))
				{
					//Thread.Sleep(1);	// 넘겨주면 안됨
				}
				_mailboxLock.ExitWriteLock();
			}

			if (result.Count <= 0)
				return false;

			return true;
		}

		public bool ContainsMail(EN_MAIL title)
		{
			return _mailList.ContainsKey(title);
		}
		public bool ContainsMail(EN_SUBSCRIBER from, EN_MAIL title)
		{
			if (false == _mailList.ContainsKey(title))
				return false;

			foreach(Mail mail in _mailList[title].ToList())
			{
				if(mail.From.Equals(from))
					return true;
			}

			return false;
		}

		public void EmptyBox()
		{
			_mailList.Clear();
		}
		public void RegistrationNotificationFunc(NotificationDelegate notificationFunc)
		{
			_notificationFunc = notificationFunc;
		}
	}
	public class Mail : ICloneable
	{
		#region <FILED>
		EN_SUBSCRIBER _from;
		EN_SUBSCRIBER _to;
		EN_MAIL _title;
		List<object> _content = null;
		#endregion </FILED>

		#region <CONSTRUCTOR>
		public Mail(EN_SUBSCRIBER from, EN_SUBSCRIBER to, EN_MAIL title, params object[] content)
		{
			_from = from;
			_to = to;
			_title = title;

			_content = new List<object>();
			for(int i=0; i<content.Length; ++i)
			{
				_content.Add(content[i]);
			}
		}
		private Mail(EN_SUBSCRIBER from, EN_SUBSCRIBER to, EN_MAIL title, List<object> content)
		{
			_from = from;
			_to = to;
			_title = title;

			_content = new List<object>();
			foreach(object data in content)
			{
				_content.Add(data);
			}
		}
		#endregion </CONSTRUCTOR>

		#region <PROPERTY>
		public EN_SUBSCRIBER From { get { return _from; } }
		public EN_SUBSCRIBER To { get { return _to; } }
		public EN_MAIL Title { get { return _title; } }
		public List<object> Content { get { return _content; } }
		public int ContentCount { get { return Content.Count; } }
		#endregion </PROPERTY>

		public object Clone()
		{
			return new Mail(_from, _to, _title, _content);
		}
	}
}
