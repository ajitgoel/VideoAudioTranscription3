/* eslint-disable */
// this is an auto generated file. This will be overwritten

export const onCreateUserProfile = /* GraphQL */ `
  subscription OnCreateUserProfile($owner: String!) {
    onCreateUserProfile(owner: $owner) {
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
export const onUpdateUserProfile = /* GraphQL */ `
  subscription OnUpdateUserProfile($owner: String!) {
    onUpdateUserProfile(owner: $owner) {
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
export const onDeleteUserProfile = /* GraphQL */ `
  subscription OnDeleteUserProfile($owner: String!) {
    onDeleteUserProfile(owner: $owner) {
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
