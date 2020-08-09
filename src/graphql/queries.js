/* eslint-disable */
// this is an auto generated file. This will be overwritten

export const getUserProfile = /* GraphQL */ `
  query GetUserProfile($id: String!) {
    getUserProfile(id: $id) {
      id
      fullName
      billingAddress
      city
      zip
      state
      country
      vatNumber
      paymentInvoices
      notificationSettings {
        notifyWhenTranscriptsCompleted
        notifyWhenTranscriptsError
      }
      paymentSettings {
        autoRecharge
      }
      transcriptionSettings {
        defaultFileLanguageWhenFileIsTranscribed
        useVocabularyWhenFileIsTranscribed
        useAutomaticContentRedaction
        vocabularies
        unwantedWords
      }
      createdAt
      updatedAt
      owner
    }
  }
`;
export const listUserProfiles = /* GraphQL */ `
  query ListUserProfiles(
    $id: String
    $filter: ModelUserProfileFilterInput
    $limit: Int
    $nextToken: String
    $sortDirection: ModelSortDirection
  ) {
    listUserProfiles(
      id: $id
      filter: $filter
      limit: $limit
      nextToken: $nextToken
      sortDirection: $sortDirection
    ) {
      items {
        id
        fullName
        billingAddress
        city
        zip
        state
        country
        vatNumber
        paymentInvoices
        notificationSettings {
          notifyWhenTranscriptsCompleted
          notifyWhenTranscriptsError
        }
        paymentSettings {
          autoRecharge
        }
        transcriptionSettings {
          defaultFileLanguageWhenFileIsTranscribed
          useVocabularyWhenFileIsTranscribed
          useAutomaticContentRedaction
          vocabularies
          unwantedWords
        }
        createdAt
        updatedAt
        owner
      }
      nextToken
    }
  }
`;
