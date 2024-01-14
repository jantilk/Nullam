import {CurrentParticipant} from "./index.tsx";

export function generateParticipantLink(
  socialEventId: string | undefined,
  participant: CurrentParticipant
): string {
  if (!socialEventId || !participant.type || !participant.id) {
    console.log(socialEventId)
    console.log(participant)
    return ""; // Handle invalid input if needed
  }

  if (participant.type === 'company') {
    return `/social-events/${socialEventId}/participating-companies/${participant.id}`;
  }

  if (participant.type === 'person') {
    return `/social-events/${socialEventId}/participating-persons/${participant.id}`;
  }

  return "";
}
