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
      }
      nextToken
    }
  }
`;