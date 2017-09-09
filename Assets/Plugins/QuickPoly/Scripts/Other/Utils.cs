using UnityEngine;
using System.Collections;

namespace QuickPoly
{
	namespace MSG
	{
		public class Warnings
		{
			public const string COLLIDER_POLYGON_BORDER_NOT_EXIST = "Can't create Polygon Collider, when Border does not exist. Collider is set to None.";
			public const string COLLIDER_POLYGON_FILL_NOT_EXIST = "Can't create Polygon Collider, when Fill does not exist. Collider is set to None.";
			public const string COLLIDER_POLYGON_FILL_AND_BORDER_NOT_EXIST = "Can't create Collider, when Fill and Border does not exist. Collider is set to None.";
            
			public const string PRESET_LAST_NOT_FOUND = "Last used preset not found. Creating a default shape.";
			public const string PRESET_MOST_USED_NOT_FOUND = "Most used preset not found. Creating a default shape.";
			public const string PRESET_NOT_FOUND = "Preset not found.";
			public const string PRESET_SAVE_EMPTY_NAME = "Can't save preset without the name";
			
			public const string SHAPEMESH_RESOLUTION_TOO_LOW = "Lower resolution can't be used. Using lowest possible resolution";
			public const string SHAPEMESH_RESOLUTION_TOO_HIGH = "Higher resolution can't be used. Using highest possible resolution";
			public const string SHAPEMESH_RESOLUTION_UNCHANGEABLE = "This shape resolution can't be changed.";
			public const string SHAPEMESH_ROUNDING_WRONG_ID = "Rounding ID is wrong";
			public const string SHAPEMESH_ROUNDING_WRONG_VALUE = "Rounding value is wrong";
			public const string SHAPEMESH_WRONG_ROUNDING_DISTANCE = "Rounding distance value is over the range.";
			public const string SHAPEMESH_IS_NOT_STAR_MESH = "This option is only available if your mesh type is a star.";
			public const string SHAPEMESH_IS_NOT_DIAMOND_MESH = "This option is only available if your mesh type is a diamond.";
			
			public const string BORDER_NOT_EXIST = "Border does not exist.";
            public const string PRESET_MULTIOBJECT_SAVING_DISABLED = "Preset saving is not allowed when selecting multiple objects.";
			public const string PRESET_MULTIOBJECT_LOADING_DISABLED = "Different UI settings don't allow for multiobject preset loading.";
            public const string SHAPE_MULTIOBJECT_SETUP_DISABLED = "Different mesh type settings don't allow for multiobject parameters setup.";
            public const string FILL_MULTIOBJECT_SETUP_DISABLED = "Different fill type settings don't allow for multiobject parameters setup.";
            public const string BORDER_MULTIOBJECT_SETUP_DISABLED = "Different border type settings don't allow for multiobject parameters setup.";
		}

		public class Errors
		{
			public const string PRESET_DELETE_NOT_EXIST = "Can't delete preset that do not exist";
			public const string PRESET_LOAD_NOT_EXIST = "Preset you tries to load does not exist";
			public const string WRONG_INTERPOLATE_TYPE = "Wrong Interpolate type";
			public const string WRONG_ROUNDING_CORNER = "Wrong corner ID used";
		}
	}


}