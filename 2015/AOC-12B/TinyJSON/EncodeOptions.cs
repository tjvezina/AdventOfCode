﻿using System;


namespace TinyJSON
{
	[Flags]
	public enum EncodeOptions
	{
		None = 0,
		PrettyPrint = 1,
		TypeHints = 2,
		IncludePublicProperties = 4,
		EnforceHeirarchyOrder = 8,
	}
}
