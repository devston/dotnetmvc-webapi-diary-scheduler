using System;

namespace DiaryScheduler.ScheduleManagement.Core.Exceptions;

public class ScheduleManagementEventNotFoundException : Exception
{
	public ScheduleManagementEventNotFoundException() : base()
	{

	}

	public ScheduleManagementEventNotFoundException(string message) : base(message)
	{
		
	}

	public ScheduleManagementEventNotFoundException(string message, params object[] args) : base (string.Format(message, args))
	{

	}
}
