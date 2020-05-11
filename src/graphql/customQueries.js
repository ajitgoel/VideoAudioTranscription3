export const listUserProfilesForGeneral = /* GraphQL */ `
  query ListUserProfiles(
    $filter: ModelUserProfileFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listUserProfiles(filter: $filter, limit: $limit, nextToken: $nextToken) {
      items {
        id
        userId
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
        userId
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
        userId
        paymentSettings {
          autoRecharge
        }
      }
      nextToken
    }
  }
`;