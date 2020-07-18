/* eslint-disable */
// this is an auto generated file. This will be overwritten

export const getAlbum = /* GraphQL */ `
  query GetAlbum($id: ID!) {
    getAlbum(id: $id) {
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
export const listAlbums = /* GraphQL */ `
  query ListAlbums(
    $filter: ModelAlbumFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listAlbums(filter: $filter, limit: $limit, nextToken: $nextToken) {
      items {
        id
        name
        Videos {
          nextToken
        }
        createdAt
        updatedAt
      }
      nextToken
    }
  }
`;
export const getVideo = /* GraphQL */ `
  query GetVideo($id: ID!) {
    getVideo(id: $id) {
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
export const listVideos = /* GraphQL */ `
  query ListVideos(
    $filter: ModelVideoFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listVideos(filter: $filter, limit: $limit, nextToken: $nextToken) {
      items {
        id
        albumId
        album {
          id
          name
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
      nextToken
    }
  }
`;
export const getUserProfile = /* GraphQL */ `
  query GetUserProfile($id: String!) {
    getUserProfile(id: $id) {
      id
      cognitoId
      fullName
      billingAddress
      country
      vatNumber
      vocabularies
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
        cognitoId
        fullName
        billingAddress
        country
        vatNumber
        vocabularies
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
      nextToken
    }
  }
`;
export const listVideosByAlbum = /* GraphQL */ `
  query ListVideosByAlbum(
    $albumId: ID
    $sortDirection: ModelSortDirection
    $filter: ModelVideoFilterInput
    $limit: Int
    $nextToken: String
  ) {
    listVideosByAlbum(
      albumId: $albumId
      sortDirection: $sortDirection
      filter: $filter
      limit: $limit
      nextToken: $nextToken
    ) {
      items {
        id
        albumId
        album {
          id
          name
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
      nextToken
    }
  }
`;
