/* eslint-disable */
// this is an auto generated file. This will be overwritten

export const onCreateAlbum = /* GraphQL */ `
  subscription OnCreateAlbum {
    onCreateAlbum {
      id
      name
      Videos {
        items {
          id
          albumId
          bucket
          labels
          createdAt
          updatedAt
        }
        nextToken
      }
      createdAt
      updatedAt
    }
  }
`;
export const onUpdateAlbum = /* GraphQL */ `
  subscription OnUpdateAlbum {
    onUpdateAlbum {
      id
      name
      Videos {
        items {
          id
          albumId
          bucket
          labels
          createdAt
          updatedAt
        }
        nextToken
      }
      createdAt
      updatedAt
    }
  }
`;
export const onDeleteAlbum = /* GraphQL */ `
  subscription OnDeleteAlbum {
    onDeleteAlbum {
      id
      name
      Videos {
        items {
          id
          albumId
          bucket
          labels
          createdAt
          updatedAt
        }
        nextToken
      }
      createdAt
      updatedAt
    }
  }
`;
export const onCreateVideo = /* GraphQL */ `
  subscription OnCreateVideo {
    onCreateVideo {
      id
      albumId
      album {
        id
        name
        Videos {
          nextToken
        }
        createdAt
        updatedAt
      }
      bucket
      fullsize {
        key
        width
        height
      }
      thumbnail {
        key
        width
        height
      }
      labels
      createdAt
      updatedAt
    }
  }
`;
export const onUpdateVideo = /* GraphQL */ `
  subscription OnUpdateVideo {
    onUpdateVideo {
      id
      albumId
      album {
        id
        name
        Videos {
          nextToken
        }
        createdAt
        updatedAt
      }
      bucket
      fullsize {
        key
        width
        height
      }
      thumbnail {
        key
        width
        height
      }
      labels
      createdAt
      updatedAt
    }
  }
`;
export const onDeleteVideo = /* GraphQL */ `
  subscription OnDeleteVideo {
    onDeleteVideo {
      id
      albumId
      album {
        id
        name
        Videos {
          nextToken
        }
        createdAt
        updatedAt
      }
      bucket
      fullsize {
        key
        width
        height
      }
      thumbnail {
        key
        width
        height
      }
      labels
      createdAt
      updatedAt
    }
  }
`;
export const onCreateUserProfile = /* GraphQL */ `
  subscription OnCreateUserProfile($owner: String!) {
    onCreateUserProfile(owner: $owner) {
      id
      fullName
      billingAddress
      country
      vatNumber
      vocabularies {
        text
        entry
      }
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
      country
      vatNumber
      vocabularies {
        text
        entry
      }
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
      country
      vatNumber
      vocabularies {
        text
        entry
      }
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
      }
      createdAt
      updatedAt
      owner
    }
  }
`;
