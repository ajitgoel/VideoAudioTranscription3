export const listUserProfilesForGeneral = /* GraphQL */ `
  query ListUserProfiles(
    $filter: ModelUserProfileFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
      items {
        id
        fullName
        billingAddress
        country
        vatNumber
        notificationTranscriptsCompleted
        notificationTranscriptsError
      }
      nextToken
    }
  }
`;

export const listUserProfilesForVocabularies = /* GraphQL */ `
  query ListUserProfiles(
    $filter: ModelUserProfileFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
      items {
        id
        vocabularies
      }
      nextToken
    }
  }
`;

export const listUserProfilesForPaymentSettings = /* GraphQL */ `
  query ListUserProfiles(
    $filter: ModelUserProfileFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
      items {
        id
        paymentSettings {
          autoRecharge
        }
      }
      nextToken
    }
  }
`;