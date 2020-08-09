/* eslint-disable */
// this is an auto generated file. This will be overwritten

export const createUserProfile = /* GraphQL */ `
  mutation CreateUserProfile(
    $input: CreateUserProfileInput!
    $condition: ModelUserProfileConditionInput
  ) {
    createUserProfile(input: $input, condition: $condition) {
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
export const updateUserProfile = /* GraphQL */ `
  mutation UpdateUserProfile(
    $input: UpdateUserProfileInput!
    $condition: ModelUserProfileConditionInput
  ) {
    updateUserProfile(input: $input, condition: $condition) {
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
export const deleteUserProfile = /* GraphQL */ `
  mutation DeleteUserProfile(
    $input: DeleteUserProfileInput!
    $condition: ModelUserProfileConditionInput
  ) {
    deleteUserProfile(input: $input, condition: $condition) {
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
