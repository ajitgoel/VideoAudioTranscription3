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
        }
        nextToken
      }
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
      }
      nextToken
    }
  }
`;
export const getUserProfile = /* GraphQL */ `
  query GetUserProfile($id: ID!) {
    getUserProfile(id: $id) {
      id
      userId
      fullName
      billingAddress
      country
      vatNumber
      notificationTranscriptsCompleted
      notificationTranscriptsError
      vocabularies
      paymentSettings {
        autoRecharge
      }
      owner
    }
  }
`;
export const listUserProfiles = /* GraphQL */ `
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
        vocabularies
        paymentSettings {
          autoRecharge
        }
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
      }
      nextToken
    }
  }
`;
