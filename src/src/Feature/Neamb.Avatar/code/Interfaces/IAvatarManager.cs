using Neambc.Neamb.Feature.Avatar.Models;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Avatar.Interfaces
{
	public interface IAvatarManager {
		AvatarDTO GetAvatarModel(string uploadParameter, Rendering rendering);
		AvatarResultOperation SaveAvatar(string data);
	}
}