import { Component, TemplateRef, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ArtistDto, PaginatedListOfArtistDto } from '../../web-api-client';
import { ArtistsClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-artist-list-component',
  templateUrl: './artist-list.component.html',
  styleUrls: ['./artist-list.component.scss']
})
export class ArtlstListComponent implements OnInit {
  artistList: ArtistDto[];

  constructor(
    private readonly artistsClient: ArtistsClient,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.artistsClient.getArtists().subscribe({
        next: result => {
          this.artistList = result.items;
        },
        error: error => console.error(error)
      });
  }
}
