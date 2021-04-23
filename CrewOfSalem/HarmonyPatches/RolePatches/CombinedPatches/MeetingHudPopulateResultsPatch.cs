using System.Linq;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.CombinedPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
    public static class MeetingHudPopulateResultsPatch
    {
        public static bool Prefix([HarmonyArgument(0)] Il2CppStructArray<byte> states)
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            
            MeetingHud.Instance.TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
            var num = 0;
            for (var i = 0; i < MeetingHud.Instance.playerStates.Length; i++)
            {
                PlayerVoteArea playerVoteArea = MeetingHud.Instance.playerStates[i];
                playerVoteArea.ClearForResults();
                var num2 = 0;
                var hasShownMayorVote = false;
                for (var j = 0; j < MeetingHud.Instance.playerStates.Count; j++)
                {
                    PlayerVoteArea playerVoteArea2 = MeetingHud.Instance.playerStates[j];
                    // Blackmailer Skip Vote
                    if (blackmailAbilities.Any(blackmail => blackmail.BlackmailedPlayer.PlayerId == playerVoteArea2.TargetPlayerId)) continue;

                    byte self = states[playerVoteArea2.TargetPlayerId];
                    if (global::Extensions.HasAnyBit(self, (byte) 128)) continue;

                    GameData.PlayerInfo playerById =
                        GameData.Instance.GetPlayerById((byte) playerVoteArea2.TargetPlayerId);
                    int votedFor = PlayerVoteArea.GetVotedFor(self);
                    if (votedFor == playerVoteArea.TargetPlayerId)
                    {
                        SpriteRenderer spriteRenderer = Object.Instantiate(MeetingHud.Instance.PlayerVotePrefab,
                            playerVoteArea.transform, true);
                        if (PlayerControl.GameOptions.AnonymousVotes)
                        {
                            PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, spriteRenderer);
                        } else
                        {
                            PlayerControl.SetPlayerMaterialColors(playerById.ColorId, spriteRenderer);
                        }


                        Transform transform = spriteRenderer.transform;
                        transform.localPosition = MeetingHud.Instance.CounterOrigin +
                                                  new Vector3(MeetingHud.Instance.CounterOffsets.x * num2, 0f, 0f);
                        transform.localScale = Vector3.zero;
                        MeetingHud.Instance.StartCoroutine(Effects.Bloop(num2 * 0.5f, transform, 1f, 0.5f));
                        num2++;
                    } else if (i == 0 && votedFor == -1)
                    {
                        SpriteRenderer spriteRenderer2 = Object.Instantiate(MeetingHud.Instance.PlayerVotePrefab,
                            MeetingHud.Instance.SkippedVoting.transform, true);
                        if (PlayerControl.GameOptions.AnonymousVotes)
                        {
                            PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, spriteRenderer2);
                        } else
                        {
                            PlayerControl.SetPlayerMaterialColors(playerById.ColorId, spriteRenderer2);
                        }

                        Transform transform = spriteRenderer2.transform;
                        transform.localPosition = MeetingHud.Instance.CounterOrigin +
                                                  new Vector3(MeetingHud.Instance.CounterOffsets.x * num, 0f, 0f);
                        transform.localScale = Vector3.zero;
                        MeetingHud.Instance.StartCoroutine(Effects.Bloop(num * 0.5f, transform, 1f, 0.5f));
                        num++;
                    }

                    // Mayor Add Extra Vote
                    if (!TryGetSpecialRole(out Mayor mayor) || (!mayor.GetAbility<AbilityReveal>()?.hasRevealed ?? true) || playerVoteArea2.TargetPlayerId != (sbyte) mayor.Owner.PlayerId || hasShownMayorVote) continue;

                    hasShownMayorVote = true;
                    j--;
                }
            }

            return false;
        }
    }
}